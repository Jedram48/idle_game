import express from "express";
import cors from "cors";
import * as dotenv from "dotenv";
import { connectToDb } from "./config/db.js";
import authRoutes from "./routes/authRoutes.js";
import questRoutes from "./routes/questRoutes.js";

dotenv.config();
connectToDb();

const app: express.Application = express();
app.use(express.json());
app.use(cors());

app.use("/auth", authRoutes);
app.use("/quests", questRoutes);

app.get("/test", (req, res) => {
  res.send("<h1>It's working 🤗</h1>");
});

app.listen(process.env.SERVER_PORT, () => {
  console.log(`Connected to server on port ${process.env.SERVER_PORT}`);
});
