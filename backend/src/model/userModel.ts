import { Schema, model } from "mongoose";
import { User } from "../types/types.js";

const userSchema = new Schema<User>({
  username: {
    type: String,
    required: [true, "Please add a username"]
  },
  email: {
    type: String,
    required: [true, "Please add an email"],
    lowercase: true,
    unique: true,
  },
  password: {
    type: String,
    required: [true, "Please add a password"]
  },
  country: {
    type: String,
  }
});

export const user_model = model<User>("users", userSchema);
