
[System.Serializable]
public class CreateResponse {
    public int code;
    public string msg;
    public GameAccount data;
}


[System.Serializable]
public class LoginData {
    public string email;
    public string password;
}


[System.Serializable]
public class RegisterData {    
    public string username;
    public string email;
    public string password;
    public string country;
}

[System.Serializable]
public class RegisterResponse {
    public string message;
    public GameAccount data;
}

[System.Serializable]
public class LoginResponse {
    public string error;
    public string accessToken;
    public string refreshToken;
    public GameAccount data;
}

[System.Serializable]
public class AuthorizeResponse {
    public string error;
    public string message;
    public string accessToken;
    public string userId;
    public string username;
}

[System.Serializable]
public class LogoutResponse {
    public string error;
    public string message;
}

