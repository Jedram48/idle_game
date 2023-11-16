import { Schema, model } from "mongoose";
import { User } from "../types/types";

const userSchema = new Schema<User>({
  name: {
    type: String,
    required: [true, "Please add a name"]
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
