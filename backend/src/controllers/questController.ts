import { Request, Response } from "express";

export const getAllQuests = async (req: Request, res: Response) => {
  res.send("<h1>Otrzymano wszystkie questy🫠</h1>");
}

export const getQuest = async (req: Request, res: Response) => {
  const id = req.params.id;
  console.log(id);
}