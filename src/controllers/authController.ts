import { Request, Response } from 'express';
import { HydratedDocument } from 'mongoose';
import bcrypt from "bcrypt";
import { user_model as UserModel } from '../model/userModel.js';
import { User } from '../types/types.js';

export const registerUser = async (req: Request, res: Response) => {
  const { name, email, password, country } = req.body;
  if(!name || !email || !password) {
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
  console.log("My hashed password is ", hashPassword);
  // Save new user to database
  const newUser: HydratedDocument<User> = await UserModel.create({
    name,
    email,
    password: hashPassword,
    country: country ? country : null,
  });
  console.log(newUser);

  if(newUser) {
    return res.status(201).json({
      userId: newUser._id,
      email: newUser.email
    });
  } else {
    return res.status(400).json({
      message: "Registering not done correctly"
    });
  }
}

export const loginUser = async (req: Request, res: Response) => {
  const { email, password } = req.body;
  if(!email || !password) return res.status(400).json({ message: "Email and password are required" });
  const user: HydratedDocument<User> = await UserModel.findOne({ email });

  if(!user) {
    return res.status(401).json({
      message: "User of this data does not exist"
    });
  }
  // Compare passwords
  let passwordMatch: boolean;
  passwordMatch = await bcrypt.compare(password, user.password);
  if(passwordMatch) {
    return res.status(200).json({
      userId: user._id,
    });
  } else {
    res.sendStatus(401);
  }
}

export const logoutUser = async (req: Request, res: Response) => {

}
