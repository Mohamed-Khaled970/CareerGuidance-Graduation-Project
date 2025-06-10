import React, { useState, useEffect } from "react";
import {
  Box,
  Typography,
  Avatar,
  TextField,
  Button,
  IconButton,
  Paper,
  Tooltip,
  Snackbar,
  Alert,
  useTheme,
  InputAdornment,
  useMediaQuery,
  List,
  ListItem,
  CircularProgress,
} from "@mui/material";
import {
  Edit as EditIcon,
  Person as PersonIcon,
  Image as ImageIcon,
  Email as EmailIcon,
  Work as WorkIcon,
} from "@mui/icons-material";
import InfoIcon from "@mui/icons-material/Info";
import { useAuth } from "context/AuthContext";
import { api } from "../../../services/axiosInstance";

function UpdateUser() {
  const [userInfo, setUserInfo] = useState({
    name: "",
    imageUrl: "",
    about: "",
  });
  const [showImagePreview, setShowImagePreview] = useState(false);
  const [openSnackbar, setOpenSnackbar] = useState(false);
  const [errorMessage, setErrorMessage] = useState("");
  const [open, setOpen] = useState(false);
  const [submitted, setSubmitted] = useState(false);
  const theme = useTheme();
  const isMobile = useMediaQuery(theme.breakpoints.down("sm"));
  const isTablet = useMediaQuery(theme.breakpoints.between("sm", "md"));
  const [isLoading, setIsLoading] = useState(false);

  const [editStates, setEditStates] = useState({
    name: false,
    imageUrl: false,
    about: false,
    all: false,
  });
  const [focused, setFocused] = useState({
    name: false,
    imageUrl: false,
    about: false,
  });
  const { user } = useAuth();
  const userId = user?.id;
  const userName = user?.userName;
  const userEmail = user?.email;
  const role = user?.role;
  const isAdmin = role=== "Admin";

  const isValidName = /^[a-zA-Z\s]{3,30}$/.test(userInfo.name);

  const nameCriteria = [
    {
      label:
        "Must contain only letters and be between 3 and 30 characters long",
      valid: isValidName,
    },
  ];

  const isValidImageUrl = /^https:\/\//i.test(userInfo.imageUrl);
  const imageUrlCriteria = [
    { label: "Must starts with https://", valid: isValidImageUrl },
  ];

  const isValidAbout =
    userInfo.about.length >= 50 && userInfo.about.length <= 300;
  const aboutCriteria = [
    {
      label: "Must be between 50 and 300 chars",
      valid: userInfo.about.length >= 50 && userInfo.about.length <= 300,
    },
  ];

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    // Do not trim here to preserve spaces within the text
    setUserInfo((prev) => ({
      ...prev,
      [name]: value,
    }));
  };

  const toggleEdit = (field) => {
    if (field === "all") {
      setEditStates((prev) => {
        const newState = !prev.all;
        return {
          name: newState,
          imageUrl: newState,
          about: newState,
          all: newState,
        };
      });
    } else {
      setEditStates((prev) => ({ ...prev, [field]: !prev[field] }));
    }
  };

  const handleKeyPress = (e) => {
    if (e.key === "Enter") {
      e.preventDefault();
      handleSave();
    }
  };

  // ✅ Fetch data on mount
  useEffect(() => {
    const fetchProfile = async () => {
      setIsLoading(true);

      try {
        const endpoint = isAdmin
        ? `/api/userProfile/GetUserById/${userId}`
        : `/api/Interview/profile/${userId}`;

      const response = await api.get(endpoint);
      const data = response.data;

        setUserInfo({
          name: data.name || "",
          imageUrl: data.imageUrl || "",
          about: data.about || "",
        });
      } catch (error) {
        console.error("Failed to fetch profile data:", error);
      } finally {
        setIsLoading(false);
      }
    };

    if (userId) {
      fetchProfile();
    }
  }, [userId]);

  const handleSave = async (e) => {
    e.preventDefault();
    setSubmitted(true);

    const isNameValid = isValidName;
    const isImageUrlValid = isValidImageUrl;
    const isAboutValid = isAdmin ? true : isValidAbout; // ✅ نتجاهل فحص about لو اليوزر أدمن

    if (isNameValid && isImageUrlValid && isAboutValid) {
      // ✅ نجهز الداتا المشتركة
      const baseData = {
        userName: userName,
        email: userEmail,
        name: userInfo.name.trim() || "",
        imageUrl: userInfo.imageUrl.trim() || "",
        instagram: "",
        facebook: "",
        linkedIn: "",
        github: "",
      };

      // ✅ نضيف حقل about فقط لو مش أدمن
      if (!isAdmin) {
        baseData.about = userInfo.about?.trim?.() || "";
      }

      try {
        const apiEndpoint = isAdmin
          ? "/api/Dashboard/UpdateAdminProfile"
          : "/api/Interview/profile/update";

        const response = await api.put(apiEndpoint, baseData);
        console.log("Profile updated successfully:", response.data);
        setOpen(true);
      } catch (error) {
        console.log("Error during API call:", error);
        if (error.response && [400, 409, 401].includes(error.response.status)) {
          const errorMessage = error.response.data.errors?.[1];
          setErrorMessage(errorMessage);
        } else {
          setErrorMessage("An unexpected error occurred.");
        }
        setOpenSnackbar(true);
      }
    }
  };

  const handleCloseSnackbar = () => {
    setOpenSnackbar(false);
    setErrorMessage("");
  };

  const handleClose = (event, reason) => {
    if (reason === "clickaway") return;
    setOpen(false);
  };

  const inputStyleFull = {
    // width: { xs: "100%", sm: "90%", md: "85%", lg: "100%" },
    "& .MuiOutlinedInput-root": {
      borderRadius: "25px",
      height: "45px",
      paddingRight: 1,
      backgroundColor: "transparent",
      "& fieldset": { border: "1px solid #ee6c4d" },
      "&:hover fieldset": { borderColor: "#ee6c4d" },
      "&.Mui-focused fieldset": { borderColor: "#ee6c4d" },
    },
    "& .MuiInputBase-input": { color: theme.palette.text.primary },
    "& .MuiInputBase-input::placeholder": {
      color: theme.palette.text.primary,
      opacity: 0.7,
    },
    "& input:-webkit-autofill": {
      WebkitBoxShadow: "0 0 0 10px transparent inset",
      backgroundColor: "transparent",
      WebkitTextFillColor: theme.palette.text.primary,
      transition: "background-color 5000s ease-in-out 0s",
    },
    "& input:-webkit-autofill:focus, & input:-webkit-autofill:hover": {
      backgroundColor: "transparent",
      WebkitBoxShadow: "0 0 0 10px transparent inset",
      transition: "background-color 5000s ease-in-out 0s",
    },
  };

  const adornmentProps = (icon) => ({
    startAdornment: (
      <InputAdornment position="start">
        <div style={{ display: "flex", alignItems: "center", height: "100%" }}>
          {React.cloneElement(icon, {
            style: { color: "#ee6c4d", fontSize: 25, ...icon.props?.style },
          })}
          <div
            style={{
              height: "30px",
              width: "2px",
              backgroundColor: "#ee6c4d",
              marginLeft: "8px",
              marginRight: "4px",
              borderRadius: "1px",
            }}
          />
        </div>
      </InputAdornment>
    ),
  });

  const typographySx = {
    display: "flex",
    alignItems: "center",
    gap: 1,
    fontSize: { xs: "0.8rem", sm: "0.9rem", md: "1rem" },
    fontWeight: 500,
    color: theme.palette.text.primary,
    overflow: "hidden",
    textOverflow: "ellipsis",
    whiteSpace: "nowrap",
    maxWidth: "100%",
  };

  const tooltipProps = {
    placement: isMobile ? "bottom" : isTablet ? "top" : "right-start",
    arrow: true,
    PopperProps: {
      sx: {
        "& .MuiTooltip-tooltip": {
          backgroundColor: "#f5f5f5",
          color: "#293241",
          fontSize: isMobile ? "12px" : "13px",
          fontWeight: "bold",
          padding: "8px 12px",
          borderRadius: "8px",
          boxShadow: "0 2px 4px rgba(0,0,0,0.2)",
        },
        "& .MuiTooltip-arrow": { color: "#f5f5f5" },
      },
      modifiers: [
        {
          name: "offset",
          options: {
            offset: isMobile ? [0, 8] : isTablet ? [0, 10] : [10, -5],
          },
        },
      ],
    },
  };
  if (isLoading) {
    return (
      <Box textAlign="center" mt={4}>
        <CircularProgress />
        <Typography>Loading data...</Typography>
      </Box>
    );
  }

  return (
    <Box
      sx={{
        display: "flex",
        height: "100vh",
        alignItems: "center",
        justifyContent: "center",
        p: 3,
      }}
    >
      <Box
        sx={{
          flex: 1,
          p: { xs: 1, sm: 2, md: 3 },
          overflowY: "auto",
          display: "flex",
          justifyContent: "center",
          alignItems: "center",
        }}
      >
        <Box
          sx={{
            display: "flex",
            flexDirection: "column",
            alignItems: "center",
            gap: 3,
            justifyContent: "center",
            width: { xs: "90%", sm: "80%", md: "70%", lg: "60%" },
            transition: "all 0.3s ease",
          }}
        >
          <Box
            sx={{
              display: "flex",
              flexDirection: "column",
              alignItems: "center",
              width: { xs: "100%", sm: "95%", md: "90%", lg: "60%" },
              maxWidth: { xs: "100%", sm: 550, md: 600, lg: 650 },
              backgroundColor: theme.palette.background.paper,
              p: { xs: 2, sm: 3, md: 4 },
              borderRadius: 2,
              boxShadow:
                theme.palette.mode === "dark"
                  ? "0 2px 8px rgba(0, 0, 0, 0.3)"
                  : "0 2px 8px rgba(0, 0, 0, 0.1)",
              transition: "all 0.3s ease",
            }}
          >
            <Avatar
              alt="Profile"
              src={userInfo.imageUrl}
              sx={{
                width: { xs: 80, sm: 100, md: 120 },
                height: { xs: 80, sm: 100, md: 120 },
                mb: 2,
                border: `2px solid ${
                  theme.palette.mode === "dark" ? "#ee6c4d" : "#333"
                }`,
                transition: "transform 0.2s ease",
                "&:hover": { transform: "scale(1.05)" },
              }}
            >
              {!userInfo.imageUrl && <PersonIcon />}
            </Avatar>
            <Box
              sx={{
                display: "flex",
                flexDirection: "column",
                alignItems: "flex-start",
                gap: 1.5,
                width: "100%",
              }}
            >
              <Typography variant="body1" sx={typographySx}>
                <PersonIcon
                  sx={{
                    color: "#ee6c4d",
                    fontSize: { xs: 18, sm: 20, md: 22 },
                  }}
                />
                <strong>Username:</strong> {userName || "N/A"}
              </Typography>
              <Typography variant="body1" sx={typographySx}>
                <EmailIcon
                  sx={{
                    color: "#ee6c4d",
                    fontSize: { xs: 18, sm: 20, md: 22 },
                  }}
                />
                <strong>Email:</strong> {userEmail || "N/A"}
              </Typography>
              <Typography variant="body1" sx={typographySx}>
                <WorkIcon
                  sx={{
                    color: "#ee6c4d",
                    fontSize: { xs: 18, sm: 20, md: 22 },
                  }}
                />
                <strong>Role:</strong> {role || "N/A"}
              </Typography>
            </Box>
          </Box>

          <Paper
            elevation={3}
            sx={{
              borderRadius: 2,
              p: { xs: 2, sm: 3, md: 4 },
              width: { xs: "100%", sm: "95%", md: "90%", lg: "60%" },
              maxWidth: { xs: "100%", sm: 550, md: 600, lg: 650 },
              minWidth: { xs: "90%", sm: "80%", md: "60%" },
              backgroundColor: theme.palette.background.paper,
              boxShadow:
                theme.palette.mode === "dark"
                  ? "0 2px 8px rgba(0, 0, 0, 0.3)"
                  : "0 2px 8px rgba(0, 0, 0, 0.1)",
              transition: "all 0.3s ease",
            }}
          >
            <Box
              sx={{
                display: "flex",
                justifyContent: "space-between",
                alignItems: "center",
                mb: 2,
              }}
            >
              <Typography
                sx={{
                  color: theme.palette.text.primary,
                  fontSize: { xs: 20, sm: 24, md: 28, lg: 35 },
                  fontWeight: "bold",
                  textShadow: "1px 1px 1px #b5adad",
                }}
              >
                Edit Profile
              </Typography>
              <IconButton onClick={() => toggleEdit("all")}>
                <EditIcon
                  sx={{
                    color: "#ee6c4d",
                    fontSize: { xs: 26, sm: 28, md: 35 },
                    fontWeight: "bold",
                    textShadow: "1px 1px 1px #b5adad",
                  }}
                />
              </IconButton>
            </Box>

            <Box sx={{ display: "flex", flexDirection: "column", gap: 2 }}>
              {editStates.all && (
                <Tooltip
                  title={
                    <Box sx={{ width: 230 }}>
                      <Typography
                        variant="h6"
                        sx={{
                          fontSize: 13,
                          fontWeight: "bold",
                          color: "#293241",
                        }}
                      >
                        ImageUrl Requirements:
                      </Typography>
                      <List>
                        {imageUrlCriteria.map((item, index) => (
                          <ListItem key={index} sx={{ padding: 0, margin: 0 }}>
                            <Typography
                              color={item.valid ? "green" : "red"}
                              sx={{
                                display: "flex",
                                alignItems: "center",
                                fontSize: "10px",
                                margin: 0,
                                padding: 0,
                              }}
                            >
                              {item.valid ? "✔" : "✖"} {item.label}
                            </Typography>
                          </ListItem>
                        ))}
                      </List>
                    </Box>
                  }
                  open={
                    editStates.all &&
                    (focused.imageUrl || (submitted && !isValidImageUrl))
                  }
                  {...tooltipProps}
                >
                  <TextField
                    label="Image URL"
                    placeholder="Enter image URL (https://...)"
                    name="imageUrl"
                    value={userInfo.imageUrl || ""}
                    onChange={handleInputChange}
                    error={!isValidImageUrl}
                    onFocus={() =>
                      setFocused((prev) => ({ ...prev, imageUrl: true }))
                    }
                    onBlur={() =>
                      setFocused((prev) => ({ ...prev, imageUrl: false }))
                    }
                    fullWidth
                    onKeyDown={handleKeyPress}
                    InputProps={adornmentProps(<ImageIcon />)}
                    InputLabelProps={{
                      style: { color: theme.palette.text.primary },
                    }}
                    sx={inputStyleFull}
                  />
                </Tooltip>
              )}

              {editStates.all ? (
                <Tooltip
                  title={
                    <Box sx={{ width: 230 }}>
                      <Typography
                        variant="h6"
                        sx={{
                          fontSize: 13,
                          fontWeight: "bold",
                          color: "#293241",
                        }}
                      >
                        Name Requirements:
                      </Typography>
                      <List>
                        {nameCriteria.map((item, index) => (
                          <ListItem key={index} sx={{ padding: 0, margin: 0 }}>
                            <Typography
                              color={item.valid ? "green" : "red"}
                              sx={{
                                display: "flex",
                                alignItems: "center",
                                fontSize: "10px",
                                margin: 0,
                                padding: 0,
                              }}
                            >
                              {item.valid ? "✔" : "✖"} {item.label}
                            </Typography>
                          </ListItem>
                        ))}
                      </List>
                    </Box>
                  }
                  open={
                    editStates.all &&
                    (focused.name || (submitted && !isValidName))
                  }
                  {...tooltipProps}
                >
                  <TextField
                    label="Name"
                    placeholder="Enter your full name"
                    name="name"
                    value={userInfo.name || ""}
                    onChange={handleInputChange}
                    onFocus={() =>
                      setFocused((prev) => ({ ...prev, name: true }))
                    }
                    onBlur={() =>
                      setFocused((prev) => ({ ...prev, name: false }))
                    }
                    fullWidth
                    onKeyDown={handleKeyPress}
                    margin="normal"
                    error={!isValidName}
                    InputProps={adornmentProps(<PersonIcon />)}
                    InputLabelProps={{
                      style: { color: theme.palette.text.primary },
                    }}
                    sx={inputStyleFull}
                  />
                </Tooltip>
              ) : (
                <Typography variant="body1" sx={typographySx}>
                  <PersonIcon
                    sx={{
                      color: "#ee6c4d",
                      fontSize: { xs: 18, sm: 20, md: 22 },
                    }}
                  />
                  <strong>Name:</strong> {userInfo.name || "N/A"}
                </Typography>
              )}

              {!isAdmin &&
                (editStates.all ? (
                  <Tooltip
                    title={
                      <Box sx={{ width: 230 }}>
                        <Typography
                          variant="h6"
                          sx={{
                            fontSize: 13,
                            fontWeight: "bold",
                            color: "#293241",
                          }}
                        >
                          About Requirements:
                        </Typography>
                        <List>
                          {aboutCriteria.map((item, index) => (
                            <ListItem
                              key={index}
                              sx={{ padding: 0, margin: 0 }}
                            >
                              <Typography
                                color={item.valid ? "green" : "red"}
                                sx={{
                                  display: "flex",
                                  alignItems: "center",
                                  fontSize: "10px",
                                  margin: 0,
                                  padding: 0,
                                }}
                              >
                                {item.valid ? "✔" : "✖"} {item.label}
                              </Typography>
                            </ListItem>
                          ))}
                        </List>
                      </Box>
                    }
                    open={
                      editStates.all &&
                      (focused.about || (submitted && !isValidAbout))
                    }
                    {...tooltipProps}
                  >
                    <TextField
                      label="About"
                      placeholder="Description about you"
                      name="about"
                      value={userInfo.about || ""}
                      onChange={handleInputChange}
                      error={!isValidAbout}
                      onFocus={() =>
                        setFocused((prev) => ({ ...prev, about: true }))
                      }
                      onBlur={() =>
                        setFocused((prev) => ({ ...prev, about: false }))
                      }
                      fullWidth
                      multiline
                      onKeyDown={handleKeyPress}
                      margin="normal"
                      InputProps={{
                        inputProps: { maxLength: 200 },
                      }}
                      InputLabelProps={{
                        style: { color: theme.palette.text.primary },
                      }}
                      sx={{
                        "& .MuiOutlinedInput-root": {
                          borderRadius: "8px",
                          "& fieldset": { border: "1px solid #ee6c4d" },
                          "&:hover fieldset": { borderColor: "#ee6c4d" },
                          "&.Mui-focused fieldset": { borderColor: "#ee6c4d" },
                        },
                      }}
                    />
                  </Tooltip>
                ) : (
                  <Typography
                    variant="body1"
                    sx={{ ...typographySx, whiteSpace: "normal" }}
                  >
                    <InfoIcon
                      sx={{
                        color: "#ee6c4d",
                        fontSize: { xs: 18, sm: 20, md: 22 },
                      }}
                    />
                    <strong>About:</strong> {userInfo.about || "N/A"}
                  </Typography>
                ))}

              {editStates.all && (
                <Box
                  sx={{
                    display: "flex",
                    justifyContent: "space-evenly",
                    mt: 2,
                  }}
                >
                  <Button
                    type="submit"
                    variant="contained"
                    onClick={handleSave}
                    onKeyDown={handleKeyPress}
                    sx={{
                      textTransform: "capitalize",
                      backgroundColor: "#ee6c4d",
                      width: { xs: "45%", sm: "60%", md: "150px" },
                      letterSpacing: "0.5px",
                      cursor: "pointer",
                      border: "1px solid transparent",
                      borderRadius: "8px",
                      transition: "all 0.3s ease",
                      "&:hover": {
                        backgroundColor: "#d65b3d",
                        transform: "scale(1.05)",
                      },
                    }}
                  >
                    Save Changes
                  </Button>
                  <Button
                    variant="outlined"
                    sx={{
                      color: "#ee6c4d",
                      border: "1px solid #ee6c4d",
                      borderRadius: "8px",
                      textTransform: "capitalize",
                      transition: "all 0.3s ease",
                      "&:hover": {
                        backgroundColor: "#d65b3d",
                        transform: "scale(1.05)",
                      },
                    }}
                    onClick={() => {
                      toggleEdit("all");
                      setTimeout(() => window.location.reload(), 1000);
                    }}
                  >
                    Cancel
                  </Button>
                </Box>
              )}
            </Box>
          </Paper>
        </Box>
      </Box>

      {/* Snackbars */}
      <Snackbar
        open={openSnackbar}
        autoHideDuration={6000}
        onClose={handleCloseSnackbar}
        anchorOrigin={{ vertical: "top", horizontal: "center" }}
      >
        <Alert
          onClose={handleCloseSnackbar}
          severity="error"
          sx={{ width: "100%" }}
        >
          {errorMessage}
        </Alert>
      </Snackbar>
      <Snackbar
        anchorOrigin={{ vertical: "top", horizontal: "center" }}
        open={open}
        autoHideDuration={3000}
        onClose={handleClose}
      >
        <Alert
          onClose={handleClose}
          severity="info"
          variant="filled"
          sx={{ width: "100%" }}
        >
          profile updated successfully
        </Alert>
      </Snackbar>
    </Box>
  );
}

export default UpdateUser;
