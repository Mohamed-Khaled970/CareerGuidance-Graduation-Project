import { useState } from "react";
import {
  Box,
  TextField,
  Button,
  Typography,
  InputAdornment,
  IconButton,
  Checkbox,
  FormControlLabel,
  Snackbar,
  Alert,
  useTheme,
} from "@mui/material";
import PersonIcon from "@mui/icons-material/Person";
import LockIcon from "@mui/icons-material/Lock";
import { Link, useNavigate } from "react-router-dom";
import { Visibility, VisibilityOff } from "@mui/icons-material";
import { api, scheduleTokenRefresh } from "services/axiosInstance";
import { useAuth } from "context/AuthContext";
import { setAccessToken, setRefreshToken } from "../../../services/auth";

const Login = () => {
  // State declarations
  const [openSnackbar, setOpenSnackbar] = useState(false);
  const [errorMessage, setErrorMessage] = useState("");
  const [rememberMe, setRememberMe] = useState(false);
  const [signInEmail, setSignInEmail] = useState("");
  const [signInPassword, setSignInPassword] = useState("");
  const [showPassword, setShowPassword] = useState(false);

  const navigate = useNavigate();
  const { login } = useAuth();
  const theme = useTheme();

  // Form submission handler
  const handleLoginSubmit = async (e) => {
    e.preventDefault(); // Prevents page refresh
    const LoginData = {
      EmailOrUsername: signInEmail,
      password: signInPassword,
    };
    try {
      const response = await api.post("/Auth/Login", LoginData);
      const { role, token: accessToken, refreshToken, expiresIn, ...otherData } = response.data;
      setAccessToken(accessToken);
      setRefreshToken(refreshToken);
      login({ role, ...otherData });
      scheduleTokenRefresh(expiresIn);
      const userRole = role.toLowerCase();
      if (userRole === "admin" || userRole === "instructor") {
        navigate("/dashboard");
      } else if (userRole === "student") {
        navigate("/");
      } else {
        navigate("/");
      }
      console.log("Login successful:", response.data);
    } catch (error) {
      console.log("Error during API call:", error);
      if (error.response && [400, 401, 409].includes(error.response.status)) {
        const errorMessage =
          error.response.data.errors?.[1] || "Invalid Email or Password.";
        setErrorMessage(errorMessage);
      } else {
        setErrorMessage("An unexpected error occurred.");
      }
      setOpenSnackbar(true); // Show error message
    }
  };

  // Snackbar close handler
  const handleCloseSnackbar = () => {
    setOpenSnackbar(false);
    setErrorMessage("");
  };

  // Password visibility toggle
  const handleToggleShowPassword = () => setShowPassword(!showPassword);

  return (
    <Box
      sx={{
        display: "flex",
        flexDirection: "column",
        alignItems: "center",
        justifyContent: "center",
        width: "100%",
        px: 4,
        backgroundColor: theme.palette.background.paper,
      }}
    >
      <Typography
        sx={{
          color: theme.palette.text.primary,
          fontSize: "35px",
          fontWeight: "bold",
          textShadow: "1px 1px 1px #b5adad",
        }}
      >
        Login
      </Typography>
      <Box
        component="form"
        onSubmit={handleLoginSubmit}
        style={{
          display: "flex",
          alignItems: "center",
          flexDirection: "column",
        }}
      >
        {/* Email/Username Field */}
        <TextField
          placeholder="Username or email"
          type="text"
          value={signInEmail}
          onChange={(e) => setSignInEmail(e.target.value)}
          required
          autoComplete="off"
          sx={{
            "& input:-webkit-autofill": {
              WebkitBoxShadow: "0 0 0 10px transparent inset",
              backgroundColor: "transparent",
              WebkitTextFillColor: "#293241",
              transition: "background-color 5000s ease-in-out 0s",
            },
            "& input:-webkit-autofill:focus, & input:-webkit-autofill:hover": {
              backgroundColor: "transparent",
              WebkitBoxShadow: "0 0 0 10px transparent inset",
              transition: "background-color 5000s ease-in-out 0s",
            },
            "& .MuiOutlinedInput-root": {
              borderRadius: "25px",
              width: "320px",
              height: "37px",
              margin: "15px 0",
              border: "1px solid gray",
              "& fieldset": { border: "none" },
            },
            "& .MuiInputBase-root": {
              "&.Mui-focused": { borderColor: "gray" },
            },
          }}
          InputProps={{
            startAdornment: (
              <InputAdornment position="start">
                <div
                  style={{
                    display: "flex",
                    alignItems: "center",
                    justifyContent: "center",
                    border: "2px solid #ee6c4d",
                    borderLeft: "none",
                    borderTop: "none",
                    borderBottom: "none",
                    borderRadius: "10px 0 0 10px",
                  }}
                >
                  <PersonIcon
                    style={{
                      color: "#ee6c4d",
                      fontSize: 30,
                      marginRight: 5,
                    }}
                  />
                </div>
              </InputAdornment>
            ),
          }}
        />
        {/* Password Field */}
        <TextField
          placeholder="Password"
          type={showPassword ? "text" : "password"}
          value={signInPassword}
          onChange={(e) => setSignInPassword(e.target.value)}
          required
          autoComplete="off"
          sx={{
            "& input:-webkit-autofill": {
              WebkitBoxShadow: "0 0 0 10px transparent inset",
              backgroundColor: "transparent",
              WebkitTextFillColor: "#293241",
              transition: "background-color 5000s ease-in-out 0s",
            },
            "& input:-webkit-autofill:focus, & input:-webkit-autofill:hover": {
              backgroundColor: "transparent",
              WebkitBoxShadow: "0 0 0 10px transparent inset",
              transition: "background-color 5000s ease-in-out 0s",
            },
            "& .MuiOutlinedInput-root": {
              borderRadius: "25px",
              width: "320px",
              height: "37px",
              border: "1px solid gray",
              "& fieldset": { border: "none" },
            },
            "& .MuiInputBase-root": {
              "&.Mui-focused": { borderColor: "gray" },
            },
          }}
          InputProps={{
            startAdornment: (
              <InputAdornment position="start">
                <div
                  style={{
                    display: "flex",
                    alignItems: "center",
                    justifyContent: "center",
                    border: "2px solid #ee6c4d",
                    borderLeft: "none",
                    borderTop: "none",
                    borderBottom: "none",
                    borderRadius: "10px 0 0 10px",
                  }}
                >
                  <LockIcon
                    style={{
                      color: "#ee6c4d",
                      fontSize: 30,
                      marginRight: 5,
                    }}
                  />
                </div>
              </InputAdornment>
            ),
            endAdornment: (
              <InputAdornment position="end">
                <IconButton
                  onClick={handleToggleShowPassword}
                  edge="end"
                  style={{ color: "gray" }}
                >
                  {showPassword ? <VisibilityOff /> : <Visibility />}
                </IconButton>
              </InputAdornment>
            ),
          }}
        />
        {/* Remember Me and Forgot Password */}
        <Box
          sx={{
            display: "flex",
            alignItems: "center",
            justifyContent: "space-between",
            width: "90%",
          }}
        >
          <FormControlLabel
            control={
              <Checkbox
                checked={rememberMe}
                onChange={(e) => setRememberMe(e.target.checked)}
                sx={(theme) => ({
                  transform: "scale(0.8)",
                  color: theme.palette.text.primary,
                  "&.Mui-checked": {
                    color: "#ee6c4d",
                  },
                })}
              />
            }
            label="Remember Me"
            sx={{
              "& .MuiFormControlLabel-label": {
                fontSize: "13px",
                color: theme.palette.text.primary,
                fontWeight: "bold",
              },
            }}
          />
          <Link
            to="/ForgotPassword"
            style={{
              textDecoration: "none",
              color: theme.palette.text.main,
              fontSize: "13px",
              fontWeight: "bold",
            }}
          >
            Forgot Password?
          </Link>
        </Box>
        {/* Submit Button */}
        <Button
          type="submit"
          variant="contained"
          sx={{
            textTransform: "capitalize",
            backgroundColor: "#ee6c4d",
            width: "150px",
            letterSpacing: "0.5px",
            margin: "10px 0px",
            cursor: "pointer",
            border: "1px solid transparent",
            borderRadius: "8px",
          }}
        >
          Login
        </Button>
        {/* Error Snackbar */}
        <Snackbar
          open={openSnackbar}
          autoHideDuration={6000}
          onClose={handleCloseSnackbar}
          anchorOrigin={{ vertical: "top", horizontal: "center" }}
        >
          <Alert
            onClose={handleCloseSnackbar}
            severity="error"
            sx={{
              width: "100%",
              backgroundColor: "#F5F5DC",
              color: "#ee6c4d",
              borderRadius: "8px",
              boxShadow: "0 4px 6px rgba(0, 0, 0, 0.2)",
              fontSize: "16px",
              textAlign: "center",
            }}
          >
            {errorMessage}
          </Alert>
        </Snackbar>
      </Box>
    </Box>
  );
};

export default Login;