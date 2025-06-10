import React, { useState, useEffect, useRef, useCallback } from "react";
import {
  Box,
  Typography,
  TextField,
  Button,
  Snackbar,
  Alert,
  useTheme,
  useMediaQuery,
  InputAdornment,
} from "@mui/material";
import {
  CheckCircle,
  Error,
  Facebook,
  Instagram,
  LinkedIn,
  GitHub,
} from "@mui/icons-material";
import { useAuth } from "context/AuthContext";
import { api } from "../../../services/axiosInstance";

const socialMediaIcons = {
  instagram: <Instagram />,
  linkedIn: <LinkedIn />,
  gitHub: <GitHub />,
  facebook: <Facebook />,
};

const getSocialMediaColor = (platform) => {
  const colors = {
    instagram: "#E1306C",
    linkedIn: "#0A66C2",
    gitHub: "#333",
    facebook: "#1877F2",
  };
  return colors[platform] || "#000";
};

const urlRegex = /^(https?:\/\/)?([\w.-]+)\.([a-z]{2,})(:[0-9]{1,5})?(\/.*)?$/i;

const SocialMediaAdmin = () => {
  const theme = useTheme();
  const isMobile = useMediaQuery(theme.breakpoints.down("sm"));
  const [socialLinks, setSocialLinks] = useState({
    instagram: "",
    linkedIn: "",
    gitHub: "",
    facebook: "",
  });
  const [tempLinks, setTempLinks] = useState({ ...socialLinks });
  const [socialErrors, setSocialErrors] = useState({});
  const [snackbarOpen, setSnackbarOpen] = useState(false);
  const [message, setMessage] = useState("");
  const [severity, setSeverity] = useState("info");
  const [userData, setUserData] = useState({});
  const originalLinksRef = useRef(null);
  const { user } = useAuth();
  const userId = user?.id;
  const role = user?.role;
  const isAdmin = role === "Admin";

  useEffect(() => {
    const fetchData = async () => {
      if (!userId) {
        setMessage("Please log in to view your profile.");
        setSeverity("warning");
        setSnackbarOpen(true);
        return;
      }

      try {
        const endpoint = isAdmin
          ? `/api/userProfile/GetUserById/${userId}`
          : `/api/Interview/profile/${userId}`;
        const res = await api.get(endpoint);
        const data = res.data;

        setUserData(data); 

        const links = {
          facebook: data.facebook || "",
          instagram: data.instagram || "",
          gitHub: data.gitHub || "",
          linkedIn: data.linkedIn || "",
        };

        setSocialLinks(links);
        setTempLinks(links);
        originalLinksRef.current = links;
      } catch (err) {
        console.error("Error fetching user data:", err);
        setMessage("Error fetching user data.");
        setSeverity("error");
        setSnackbarOpen(true);
      }
    };

    fetchData();
  }, [userId]);

  const validateLink = useCallback((platform, value) => {
    const trimmed = value.trim();
    if (!trimmed) return "";
    if (!urlRegex.test(trimmed)) return "Invalid URL!";
    if (!trimmed.toLowerCase().includes(platform.toLowerCase())) {
      return `Link must contain "${platform}"`;
    }
    return "";
  }, []);

  const handleTempSocialInputChange = (platform, e) => {
    const value = e.target.value;
    const error = validateLink(platform, value);

    setTempLinks((prev) => ({ ...prev, [platform]: value }));
    setSocialErrors((prev) => ({ ...prev, [platform]: error }));
  };

  const handleSave = async () => {
    const newErrors = {};
    Object.entries(tempLinks).forEach(([platform, value]) => {
      const error = validateLink(platform, value);
      if (error) newErrors[platform] = error;
    });

    if (Object.keys(newErrors).length > 0) {
      setSocialErrors(newErrors);
      setMessage("Please fix the errors before saving.");
      setSeverity("error");
      setSnackbarOpen(true);
      return;
    }

    try {
      const updatedData = {
        ...userData,  
        ...tempLinks, 
      };
      const apiEndpoint = isAdmin
        ? "/api/Dashboard/UpdateAdminProfile"
        : "/api/Interview/profile/update";

      const response = await api.put(apiEndpoint, updatedData);
      console.log("social media updated successfully:", response.data);

      setSocialLinks(tempLinks);
      originalLinksRef.current = tempLinks;
      setMessage("Links saved successfully!");
      setSeverity("success");
      setSnackbarOpen(true);
    } catch (error) {
      console.error("Error saving links:", error);
      setMessage("Failed to save links.");
      setSeverity("error");
      setSnackbarOpen(true);
    }
  };

  const handleCancel = () => {
    setTempLinks(originalLinksRef.current);
    setSocialErrors({});
    setMessage("Changes cancelled.");
    setSeverity("info");
    setSnackbarOpen(true);
  };

  return (
    <form
      onSubmit={(e) => {
        e.preventDefault();
        handleSave();
      }}
    >
      <Box
        sx={{
          pt: { xs: 8, sm: 10 },
          pl: { xs: 0, sm: 32 },
          display: "flex",
          justifyContent: "center",
          alignItems: "center",
          width: { xs: "100%", sm: "80%", md: "40%" }, // Position from second code
          maxWidth: { xs: "100%", sm: "500px", md: "600px" }, // Position from second code
          mx: "auto",
          p: { xs: 2, sm: 3, md: 5 }, // Position from second code
          borderRadius: "20px",
          boxShadow: "0 8px 32px rgba(0, 0, 0, 0.2)", // Design from first code
          bgcolor: theme.palette.background.paper,
          border: `1px solid ${theme.palette.divider}`,
          opacity: 0,
          transform: "translateY(20px)",
          animation: "fadeInUp 0.6s ease-out forwards",
          "@keyframes fadeInUp": {
            to: {
              opacity: 1,
              transform: "translateY(0)",
            },
          },
        }}
      >
        {/* Main container */}
        <Box
          sx={{
            width: "100%", // Ensure content fits within the parent Box
          }}
        >
          {/* Title with animated underline */}
          <Box sx={{ textAlign: "center", mb: { xs: 1, sm: 2 } }}>
            <Typography
              variant="h4"
              sx={{
                color: theme.palette.text.primary,
                fontWeight: "bold",
                fontSize: { xs: "1.8rem", sm: "2.2rem", md: "2.5rem" }, // Design from first code
                position: "relative",
                pb: 1,
                opacity: 0,
                animation: "fadeInDown 0.5s ease-out forwards",
                "@keyframes fadeInDown": {
                  to: {
                    opacity: 1,
                  },
                },
              }}
            >
              Social Media Links
              <Box
                sx={{
                  position: "absolute",
                  bottom: 0,
                  left: "50%",
                  transform: "translateX(-50%)",
                  width: "60px",
                  height: "4px",
                  bgcolor: "#ee6d4f", // Design from first code
                  borderRadius: "2px",
                }}
              />
            </Typography>
          </Box>

          {/* Social media input fields */}
          <Box
            sx={{
              display: "flex",
              flexDirection: "column",
              gap: { xs: 1.5, sm: 2 },
            }}
          >
            {Object.entries(tempLinks).map(([platform, value], index) => (
              <Box
                key={platform}
                sx={{
                  opacity: 0,
                  animation: `fadeIn 0.5s ease-out ${
                    0.2 + index * 0.1
                  }s forwards`,
                  "@keyframes fadeIn": {
                    to: { opacity: 1 },
                  },
                }}
              >
                <TextField
                  placeholder={`Add your ${platform} link`}
                  value={value}
                  onChange={(e) => handleTempSocialInputChange(platform, e)}
                  fullWidth
                  error={!!socialErrors[platform]}
                  helperText={socialErrors[platform]}
                  variant="outlined"
                  sx={{
                    "& .MuiOutlinedInput-root": {
                      borderRadius: "12px", // Design from first code
                      bgcolor: theme.palette.background.paper,
                      border: `1px solid ${theme.palette.divider}`, // Design from first code
                      transition: "all 0.3s ease",
                      "&:hover": {
                        borderColor: "#ee6d4f", // Design from first code
                        boxShadow: "0 0 8px rgba(238, 109, 79, 0.3)", // Design from first code
                      },
                      "&.Mui-focused": {
                        borderColor: "#ee6d4f", // Design from first code
                        boxShadow: "0 0 12px rgba(238, 109, 79, 0.5)", // Design from first code
                      },
                      "& .MuiOutlinedInput-notchedOutline": {
                        border: "none",
                      },
                      "& .MuiInputBase-input": {
                        color: theme.palette.text.primary,
                        fontSize: { xs: "0.9rem", sm: "1rem" }, // Position from second code
                        p: { xs: "10px 12px", sm: "12px 14px" }, // Design from first code
                      },
                      "& input:-webkit-autofill": {
                        WebkitBoxShadow: "0 0 0 10px transparent inset",
                        backgroundColor: "transparent",
                        WebkitTextFillColor: theme.palette.text.primary,
                        transition: "background-color 5000s ease-in-out 0s",
                      },
                      "& input:-webkit-autofill:focus, & input:-webkit-autofill:hover":
                        {
                          backgroundColor: "transparent",
                          WebkitBoxShadow: "0 0 0 10px transparent inset",
                          transition: "background-color 5000s ease-in-out 0s",
                        },
                    },
                    "& .MuiFormHelperText-root": {
                      color: "#f44336",
                      fontSize: { xs: "0.75rem", sm: "0.8rem" }, // Position from second code
                      ml: 1,
                    },
                  }}
                  InputProps={{
                    startAdornment: (
                      <InputAdornment position="start">
                        <Box
                          sx={{
                            color: getSocialMediaColor(platform),
                            fontSize: { xs: "1.2rem", sm: "1.4rem" }, // Design from first code
                            mr: 1,
                          }}
                        >
                          {socialMediaIcons[platform]}
                        </Box>
                      </InputAdornment>
                    ),
                    endAdornment: (
                      <InputAdornment position="end">
                        {socialErrors[platform] ? (
                          <Error
                            sx={{ color: "#f44336", fontSize: "1.2rem" }}
                          />
                        ) : value && !socialErrors[platform] && value.trim() ? (
                          <CheckCircle
                            sx={{ color: "#4caf50", fontSize: "1.2rem" }}
                          />
                        ) : null}
                      </InputAdornment>
                    ),
                  }}
                />
              </Box>
            ))}
          </Box>

          {/* Save and Cancel buttons */}
          <Box
            sx={{
              display: "flex",
              gap: 2,
              justifyContent: "center",
              mt: { xs: 2, sm: 3, md: 4 },
            }}
          >
            <Button
              variant="contained"
              onClick={handleSave}
              sx={{
                bgcolor: "#ee6d4f", // Design from first code
                color: "#fff",
                borderRadius: "12px", // Design from first code
                px: { xs: 3, sm: 4 }, // Design from first code
                py: { xs: 1, sm: 1.2 }, // Position from second code
                fontSize: { xs: "0.9rem", sm: "1rem" }, // Position from second code
                fontWeight: "bold",
                transition: "all 0.3s ease",
                width: { xs: "40%", sm: "150px" }, // Position from second code
                "&:hover": {
                  bgcolor: "#d95b38", // Design from first code
                  transform: "translateY(-2px)",
                  boxShadow: "0 4px 12px rgba(238, 109, 79, 0.4)", // Design from first code
                },
              }}
            >
              Save
            </Button>
            <Button
              variant="outlined"
              onClick={handleCancel}
              sx={{
                borderColor: "#ee6d4f", // Design from first code
                color: "#ee6d4f", // Design from first code
                borderRadius: "12px", // Design from first code
                px: { xs: 3, sm: 4 }, // Design from first code
                py: { xs: 1, sm: 1.2 }, // Position from second code
                fontSize: { xs: "0.9rem", sm: "1rem" }, // Position from second code
                fontWeight: "bold",
                transition: "all 0.3s ease",
                width: { xs: "40%", sm: "150px" }, // Position from second code
                "&:hover": {
                  borderColor: "#ee6d4f", // Design from first code
                  color: "#ee6d4f", // Design from first code
                  transform: "translateY(-2px)",
                  boxShadow: "0 4px 12px rgba(0, 0, 0, 0.2)", // Design from first code
                },
              }}
            >
              Cancel
            </Button>
          </Box>

          {/* Snackbar for user feedback */}
          <Snackbar
            open={snackbarOpen}
            autoHideDuration={2500}
            onClose={() => setSnackbarOpen(false)}
            anchorOrigin={{
              vertical: isMobile ? "bottom" : "top",
              horizontal: "center",
            }}
            sx={{
              width: { xs: "90%", sm: "auto" }, // Position from second code
              maxWidth: "500px", // Position from second code
              opacity: 0,
              animation: "fadeIn 0.3s ease-out forwards",
              "@keyframes fadeIn": {
                to: { opacity: 1 },
              },
            }}
          >
            <Alert
              onClose={() => setSnackbarOpen(false)}
              severity={severity}
              sx={{
                borderRadius: "8px",
                bgcolor: severity === "success" ? "#4caf50" : "#f44336", // Design from first code
                color: "#fff",
                fontSize: { xs: "0.9rem", sm: "1rem" }, // Position from second code
                width: "100%",
                p: { xs: 1, sm: 1.5 }, // Position from second code
                boxShadow: "0 4px 12px rgba(0, 0, 0, 0.3)", // Design from first code
              }}
            >
              {message}
            </Alert>
          </Snackbar>
        </Box>
      </Box>
    </form>
  );
};

export default SocialMediaAdmin;
