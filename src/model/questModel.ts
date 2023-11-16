import { Schema, model } from "mongoose";
import { Quest } from "../types/types.js";

const questSchema = new Schema<Quest>({
  name: {
    type: String,
    required: [true, "Add a quest's name"]
  },
  level: {
    type: String,
    required: [true, "Add a quest's level"],
  },
  description: {
    type: String,
    required: [true, "Add a quest's level"],
  }
});

export const quest_model = model<Quest>("quests", questSchema);
