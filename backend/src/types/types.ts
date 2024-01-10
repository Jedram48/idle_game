import { Types } from "mongoose";
import { Request } from "express";

export enum QuestType {
  EASY = "EASY",
  MEDIUM = "MEDIUM",
  INTERMEDIATE = "INTERMEDIATE",
  HARD = "HARD",
}

export interface User {
  username: string,
  email: string,
  password: string, 
  country?: string,
}

export interface Quest {
  name: string,
  level: QuestType,
  description: string,
}

export type Score = {
  userId: Types.ObjectId,
}

export interface MyRequest extends Request {
  username: string
  token: string
}

export interface UserToken {
  userId: string,
  username: string,
}

export interface Verification {
  valid: boolean,
  expired?: boolean,
  decoded: UserToken | null,
}
