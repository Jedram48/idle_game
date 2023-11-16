import { Request, Response } from "express";

export const getAllQuests = async (req: Request, res: Response) => {
  
}

export const getQuest = async (req: Request, res: Response) => {
  const id = req.params.id;
  console.log(id);
}