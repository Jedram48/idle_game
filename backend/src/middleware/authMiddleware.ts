import { Response, NextFunction } from 'express';
import { MyRequest } from '../types/types.js';
import { signJWT, verifyJWT } from '../config/jwt.js';
import { verifyRefreshToken } from '../config/jwt.js';

export const verifyToken = (req: MyRequest, res: Response, next: NextFunction) => {
  const bearerHeader = req.headers["authorization"];
  console.log(bearerHeader);

  if(typeof bearerHeader !== "undefined") {
    const bearerToken = bearerHeader.split(" ")[1];
    const response = verifyJWT(bearerToken);

    if(!response.valid) {
      // check if user sends refreshToken 
      const refreshTokenResponse = verifyRefreshToken(bearerToken);
      if(!refreshTokenResponse.valid) {
        return res.status(403).json({
          error: "Not authorized"
        });
      } else { // Create new accessToken if refreshToken is valid
        const newAccessToken = signJWT(refreshTokenResponse.decoded, {
          expiresIn: "1h"
        });
        req.username = refreshTokenResponse.decoded.username;
        req.token = newAccessToken;
        return next();
      }
    } else { // user has valid accessToken
      req.username = response.decoded.username;
      req.token = bearerToken;  
      return next();
    }
  } else {
    return res.status(403).json({
      message: "Invalid request"
    });
  }
}
