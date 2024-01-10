import { Request, Response } from 'express';
import { HydratedDocument } from 'mongoose';
import bcrypt from "bcrypt";
import { user_model as UserModel } from '../model/userModel.js';
import { MyRequest, User, UserToken } from '../types/types.js';
import { signJWT, signRefreshToken } from '../config/jwt.js';

export const registerUser = async (req: Request, res: Response) => {
  const { username, email, password, country } = req.body;
  if(!username || !email || !password) {
    return res.status(400).json({
      message: "Invalid register data - please add all the fields"
    });
  }
  // Checking if users exists
  const user: HydratedDocument<User> = await UserModel.findOne({ email });
  if(user) {
    return res.status(400).json({
      message: "User of this email already exists"
    });
  }
  let hashPassword: string = "";
  hashPassword = await bcrypt.hash(password, 10);
  // Save new user to database
  const newUser: HydratedDocument<User> = await UserModel.create({
    username,
    email,
    password: hashPassword,
    country: country ? country : null,
  });

  if(newUser) {
    return res.status(201).json({
      username: newUser.username
    });
  } else {
    return res.status(400).json({
      message: "Registering not done correctly"
    });
  }
}

export const loginUser = async (req: Request, res: Response) => {
  const { email, password } = req.body;
  if(!email || !password) return res.status(400).json({ error: "Email and password are required" });
  const user: HydratedDocument<User> = await UserModel.findOne({ email });

  if(!user) {
    console.log("Nie znaleziono usera o takim emailu...");
    return res.status(401).json({
      error: "Invalid login data"
    });
  }
  // Compare passwords
  let passwordMatch: boolean;
  passwordMatch = await bcrypt.compare(password, user.password);

  if(!passwordMatch) {
    return res.status(401).json({
      error: "Authentication failed"
    });
  }

  const payload: UserToken = {
    userId: (user._id).toString(),
    username: user.username,
  };
  const accessToken: string = signJWT(payload, {
    expiresIn: "1h"
  });
  const refreshToken: string = signRefreshToken(payload, {
    expiresIn: "3d"
  });

  return res.status(200).json({
    accessToken,
    refreshToken,
    username: user.username,
  });
}

export const authorizeUser = async (req: MyRequest, res: Response) => {
  const token = req.token;
  const username = req.username;
  if(token && username) {
    return res.status(200).json({
      message: "Authorized",
      accessToken: token,
      username,
    });
  } else {
    return res.status(403).json({
      error: "Not authorized"
    });
  }
}


export const logoutUser = async (req: Request, res: Response) => {

}
