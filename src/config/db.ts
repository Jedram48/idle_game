import mongoose from "mongoose";

export let conn: mongoose.Connection;

export const connectToDb: () => Promise<void> = async () => {
  try {
    const connection = await mongoose.connect(process.env.MONGO_CONN_STRING);
    conn = connection;
    console.log(`Database MongoDb connected: ${connection.connection.host}`);
  } catch(err) {
    console.log(err);
    process.exit(0);
  }
}