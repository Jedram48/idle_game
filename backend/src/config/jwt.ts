import jwt from "jsonwebtoken";
import * as fs from "fs";
import path from "path";
import { UserToken, Verification } from "../types/types.js";

const __dirname: string =  path.dirname(new URL(import.meta.url).pathname);
const privateKey: string = fs.readFileSync(`${__dirname}/private_key.pem`, 'utf-8');
const publicKey: string = fs.readFileSync(`${__dirname}/public_key.pem`, 'utf-8');

export const signJWT = (object: UserToken, options: any) => {
  const token: string = jwt.sign(object, privateKey, {
    ...(options && options),
    algorithm: "RS256",
  });
  return token;
}

export const signRefreshToken = (object: UserToken, options: any) => {
  const token: string = jwt.sign(object, process.env.REFRESH_TOKEN_SECRET, {
    ...(options && options)
  });
  return token;
}

export const verifyRefreshToken = (token: string) => {
  try {
    const decoded = jwt.verify(token, process.env.REFRESH_TOKEN_SECRET) as UserToken;
    const verificationResponse: Verification = {
      valid: true,
      expired: false,
      decoded
    }
    return verificationResponse;
  } catch(err) {
    const errorResponse: Verification = {
      valid: false,
      decoded: null
    }
    return errorResponse;
  }
}

export const verifyJWT = (token: string) => {
  try {
    const decoded = jwt.verify(token, publicKey, { algorithms: ['RS256'] }) as UserToken;
    const verificationResponse: Verification = {
      valid: true,
      expired: false,
      decoded
    }
    return verificationResponse;
  } catch(err) {
    const errorResponse: Verification = {
      valid: false,
      decoded: null
    }
    return errorResponse;
  }
}