using System;
using UnityEngine;

public class TokenOrganizer : MonoBehaviour {
  
  private string accessTokenTag = "ACCESS_TOKEN";
  private string usernameTag = "Username";
  private string userIdTag = "User_Id";
  private string accessTokenExpiration = "AccessTokenExpiration";
  private string refreshTokenTag = "REFRESH_TOKEN";
  private string refreshTokenExpiration = "RefreshTokenExpiration";

  public void CheckTokens() {
    CheckTokensValidity();
  }

  public void EraseTokens() {
    DeleteTokens();
  }

  public string GetAccessTokenTag() {
    return accessTokenTag;
  }

  public string GetRefreshTokenTag() {
    return refreshTokenTag;
  }

  private void CheckTokensValidity() {
    string accessTokenTimestamp = PlayerPrefs.GetString(accessTokenExpiration, string.Empty);
    string refreshTokenTimestamp = PlayerPrefs.GetString(refreshTokenExpiration, string.Empty);

    if(!string.IsNullOrEmpty(accessTokenTimestamp)) {
      DateTime accesstimestamp = DateTime.Parse(accessTokenTimestamp);
      TimeSpan accessTokenElapsedTime = DateTime.Now - accesstimestamp;

      TimeSpan accessTokenExpirationTime = new TimeSpan(1, 0, 0); // 1hour for accessToken

      if(accessTokenElapsedTime > accessTokenExpirationTime) { // accessToken is not valid
        PlayerPrefs.DeleteKey(accessTokenExpiration);
        PlayerPrefs.DeleteKey(accessTokenTag);
      }
    }

    if(!string.IsNullOrEmpty(refreshTokenTimestamp)) {
      DateTime refreshtimestamp = DateTime.Parse(refreshTokenTimestamp);
      TimeSpan refreshTokenElapsedTime = DateTime.Now - refreshtimestamp;
      TimeSpan refreshTokenExpirationTime = new TimeSpan(3, 0, 0, 0); // 3 days for refreshToken

      if(refreshTokenElapsedTime > refreshTokenExpirationTime) { // refreshToken is not valid
        PlayerPrefs.DeleteKey(refreshTokenExpiration);
        PlayerPrefs.DeleteKey(refreshTokenTag);
      }
    }
  }

  private void DeleteTokens() {
    PlayerPrefs.DeleteKey(accessTokenTag);
    PlayerPrefs.DeleteKey(accessTokenExpiration);
    PlayerPrefs.DeleteKey(refreshTokenTag);
    PlayerPrefs.DeleteKey(refreshTokenExpiration);
    PlayerPrefs.DeleteKey(usernameTag);
    PlayerPrefs.DeleteKey(userIdTag);
  }

}
