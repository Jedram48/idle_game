import { Types } from "mongoose"

export enum QuestType {
  EASY = "EASY",
  MEDIUM = "MEDIUM",
  INTERMEDIATE = "INTERMEDIATE",
  HARD = "HARD",
}

export interface User {
  name: string,
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