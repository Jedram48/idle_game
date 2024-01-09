import express from "express";
import { loginUser, logoutUser, registerUser, authorizeUser } from "../controllers/authController.js";
import { verifyToken } from "../middleware/authMiddleware.js";

const router: express.Router = express.Router();

router.post("/", registerUser);
router.post("/login", loginUser);
router.post("/verify", verifyToken, authorizeUser);
router.get("/logout", logoutUser);

export default router;