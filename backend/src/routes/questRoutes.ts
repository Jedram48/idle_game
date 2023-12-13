import express from "express";
import { getAllQuests, getQuest } from "../controllers/questController.js";

const router: express.Router = express.Router();

router.get("/", getAllQuests);
router.get("/:id", getQuest);

export default router;
