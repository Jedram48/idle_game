import express from "express";
import { loginUser, logoutUser, registerUser } from "../controllers/authController";

const router: express.Router = express.Router();

router.post("/", registerUser);
router.post("/login", loginUser);
router.get("/logout", logoutUser);

export default router;