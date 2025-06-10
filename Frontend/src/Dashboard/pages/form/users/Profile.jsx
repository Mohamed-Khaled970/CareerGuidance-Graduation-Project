import React, { useState, useEffect } from "react";
import {
  Box,
  Typography,
  Card,
  Grid,
  Avatar,
  Button,
  useTheme,
  CircularProgress,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogContentText,
  DialogActions,
  Snackbar,
  Alert,
  IconButton,
} from "@mui/material";
import { useParams, useNavigate } from "react-router-dom";
import FacebookIcon from "@mui/icons-material/Facebook";
import InstagramIcon from "@mui/icons-material/Instagram";
import LinkedInIcon from "@mui/icons-material/LinkedIn";
import GitHubIcon from "@mui/icons-material/GitHub";
import { api } from "../../../../services/axiosInstance";

export default function UserProfile() {
  const { id } = useParams();
  const navigate = useNavigate();
  const [userData, setUserData] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [confirmOpen, setConfirmOpen] = useState(false);
  const [snackbar, setSnackbar] = useState({
    open: false,
    message: "",
    severity: "success",
  });
  const theme = useTheme();

  // Helper to format ISO date to locale string
  const formatDate = (dateString) => {
    if (!dateString) return null;
    const date = new Date(dateString);
    if (isNaN(date) || date.getFullYear() < 1000) return null;
    return date.toLocaleDateString(undefined, {
      year: "numeric",
      month: "numeric",
      day: "numeric",
    });
  };

  useEffect(() => {
    const fetchUser = async () => {
      setLoading(true);
      setError(null);

      try {

        const response = await api.get(
          `/api/Dashboard/GetUserById/${id}`,
        );

        const u = response.data;
        setUserData({
          id: u.id || id,
          userName: u.userName || "null",
          name: u.name || "null",
          email: u.email || "null",
          role: u.role || "null",
          image: u.imageUrl || "null",
          country: u.country || "null",
          phoneNumber: u.phoneNumber || "null",
          dateOfBirth: formatDate(u.dateOfBirth),
          facebook: u.facebook || "null",
          gitHub: u.gitHub || "null",
          instagram: u.instagram || "null",
          linkedIn: u.linkedIn || "null",
          roadmaps_: u.roadmaps_, // array from response
        });
        console.log(u);
      } catch (err) {
        if (err.response?.status === 404) {
          setError("User not found. Redirecting...");
          setTimeout(() => navigate("/dashboard/allusers"), 3000);
        } else {
          setError(
            err.message || "Failed to fetch user data. Try again later."
          );
        }
      } finally {
        setLoading(false);
      }
    };

    fetchUser();
  }, [id, navigate]);

  const handleDeleteUser = async () => {
    try {

      await api.delete(
        `/api/Dashboard/DeleteUser/${id}`,
      );

      setSnackbar({
        open: true,
        message: "User deleted successfully.",
        severity: "success",
      });
      setTimeout(() => navigate("/dashboard/allusers"), 2000);
    } catch (err) {
      let msg = "An unknown error occurred. Please try again.";
      if (err.response?.data?.errors) {
        msg = err.response.data.errors.map((e) => e.message).join(", ");
      } else if (err.message === "No authentication token found") {
        msg = "Session expired. Please login again.";
      }

      setSnackbar({ open: true, message: msg, severity: "error" });
      console.error(err);
    }
  };

  const handleOpenConfirm = () => setConfirmOpen(true);
  const handleCloseConfirm = () => setConfirmOpen(false);
  const handleConfirmDelete = () => {
    handleDeleteUser();
    handleCloseConfirm();
  };
  const handleCloseSnackbar = (_, reason) => {
    if (reason === "clickaway") return;
    setSnackbar((s) => ({ ...s, open: false }));
  };

  if (loading) {
    return (
      <Box textAlign="center" mt={4}>
        <CircularProgress />
        <Typography>Loading user...</Typography>
      </Box>
    );
  }

  if (error) {
    return (
      <Box textAlign="center" mt={4}>
        <Typography color="error">{error}</Typography>
        <Button
          variant="contained"
          sx={{ mt: 2 }}
          onClick={() => navigate("/dashboard/allusers")}
        >
          Back to Users
        </Button>
      </Box>
    );
  }

  return (
    <Box sx={{ maxWidth: 1000, margin: "auto" }}>
      <Typography
        variant="h4"
        gutterBottom
        sx={{
          color: theme.palette.text.primary,
          fontWeight: "bold",
          textAlign: "center",
        }}
      >
        Profile
      </Typography>
      <Box textAlign="center" mb={4}>
        <Avatar
          src={userData.image}
          alt={userData.userName[0].toUpperCase()}
          sx={{
            width: 100,
            height: 100,
            margin: "auto",
            backgroundColor: "gray",
            fontSize: "2rem",
            boxShadow: "0px 4px 10px rgba(0, 0, 0, 0.2)",
          }}
        ></Avatar>
      </Box>
      <Section title="Information">
        <CardContentGrid
          data={[
            { label: "Username:", field: "userName" },
            { label: "Name:", field: "name" },
            { label: "Role:", field: "role" },
          ]}
          userData={userData}
        />
      </Section>
      <Section title="Communication">
        <CardContentGrid
          data={[
            { label: "Email:", field: "email" },
            { label: "Country:", field: "country" },
            { label: "Phone Number:", field: "phoneNumber" },
            { label: "Date of Birth:", field: "dateOfBirth" },
          ]}
          userData={userData}
        />
      </Section>
      <Section title="Social Media">
        <CardContentGrid
          data={[
            {
              label: (
                <Box
                  sx={{
                    display: "flex",
                    alignItems: "center",
                    backgroundColor: theme.palette.background.paper,
                    borderRadius: "8px",
                    padding: "8px",
                  }}
                >
                  <IconButton
                    href={userData.facebook !== "null" ? userData.facebook : "#"}
                    target="_blank"
                    rel="noopener noreferrer"
                    disabled={userData.facebook === "null"}
                    sx={{
                      color: "#3b5998",
                      "&:hover": { color: "#3b5998" },
                      "&.Mui-disabled": { color: "gray", opacity: 0.5, pointerEvents: "none" },
                      mr: 1,
                    }}
                  >
                    <FacebookIcon />
                  </IconButton>
                  <Typography
                    sx={{
                      fontSize: "1rem",
                      fontWeight: 500,
                      color: userData.facebook === "null" ? "gray" : theme.palette.text.primary,
                    }}
                  >
                    Facebook
                  </Typography>
                </Box>
              ),
              field: null,
            },
            {
              label: (
                <Box
                  sx={{
                    display: "flex",
                    alignItems: "center",
                    backgroundColor: theme.palette.background.paper,
                    borderRadius: "8px",
                    padding: "8px",
                  }}
                >
                  <IconButton
                    href={userData.gitHub !== "null" ? userData.gitHub : "#"}
                    target="_blank"
                    rel="noopener noreferrer"
                    disabled={userData.gitHub === "null"}
                    sx={{
                      color: "#000",
                      "&:hover": { color: "#000" },
                      "&.Mui-disabled": { color: "gray", opacity: 0.5, pointerEvents: "none" },
                      mr: 1,
                    }}
                  >
                    <GitHubIcon />
                  </IconButton>
                  <Typography
                    sx={{
                      fontSize: "1rem",
                      fontWeight: 500,
                      color: userData.gitHub === "null" ? "gray" : theme.palette.text.primary,
                    }}
                  >
                    GitHub
                  </Typography>
                </Box>
              ),
              field: null,
            },
            {
              label: (
                <Box
                  sx={{
                    display: "flex",
                    alignItems: "center",
                    backgroundColor: theme.palette.background.paper,
                    borderRadius: "8px",
                    padding: "8px",
                  }}
                >
                  <IconButton
                    href={userData.instagram !== "null" ? userData.instagram : "#"}
                    target="_blank"
                    rel="noopener noreferrer"
                    disabled={userData.instagram === "null"}
                    sx={{
                      color: "#C13584",
                      "&:hover": { color: "#C13584" },
                      "&.Mui-disabled": { color: "gray", opacity: 0.5, pointerEvents: "none" },
                      mr: 1,
                    }}
                  >
                    <InstagramIcon />
                  </IconButton>
                  <Typography
                    sx={{
                      fontSize: "1rem",
                      fontWeight: 500,
                      color: userData.instagram === "null" ? "gray" : theme.palette.text.primary,
                    }}
                  >
                    Instagram
                  </Typography>
                </Box>
              ),
              field: null,
            },
            {
              label: (
                <Box
                  sx={{
                    display: "flex",
                    alignItems: "center",
                    backgroundColor: theme.palette.background.paper,
                    borderRadius: "8px",
                    padding: "8px",
                  }}
                >
                  <IconButton
                    href={userData.linkedIn !== "null" ? userData.linkedIn : "#"}
                    target="_blank"
                    rel="noopener noreferrer"
                    disabled={userData.linkedIn === "null"}
                    sx={{
                      color: "#0077b5",
                      "&:hover": { color: "#0077b5" },
                      "&.Mui-disabled": { color: "gray", opacity: 0.5, pointerEvents: "none" },
                      mr: 1,
                    }}
                  >
                    <LinkedInIcon />
                  </IconButton>
                  <Typography
                    sx={{
                      fontSize: "1rem",
                      fontWeight: 500,
                      color: userData.linkedIn === "null" ? "gray" : theme.palette.text.primary,
                    }}
                  >
                    LinkedIn
                  </Typography>
                </Box>
              ),
              field: null,
            },
          ]}
          userData={userData}
        />
      </Section>
      {Array.isArray(userData.roadmaps_) &&
        userData.roadmaps_.length > 0 &&
        userData.role !== "Admin" && (
          <Section title="Roadmaps">
            <Grid container spacing={2}>
              {userData.roadmaps_.map((rm, i) => (
                <Grid item xs={12} sm={6} key={i}>
                  <Box
                    display="flex"
                    alignItems="center"
                    justifyContent="flex-start"
                    sx={{ mb: 2 }}
                  >
                    <Typography
                      fontWeight="bold"
                      sx={{
                        flex: 1,
                        mr: 1,
                        overflow: "hidden",
                        textOverflow: "ellipsis",
                        whiteSpace: "nowrap",
                      }}
                    >
                      {rm.roadmapName ?? `Roadmap ${i + 1}`}
                    </Typography>
                    <Box sx={{ position: "relative", display: "inline-flex" }}>
                      <CircularProgress
                        variant="determinate"
                        value={rm.progress || 0}
                        size={80}
                        thickness={5}
                        sx={{
                          color: "#ee6d4f",
                        }}
                      />
                      <Box
                        sx={{
                          top: 0,
                          left: 0,
                          bottom: 0,
                          right: 0,
                          position: "absolute",
                          display: "flex",
                          alignItems: "center",
                          justifyContent: "center",
                        }}
                      >
                        <Typography
                          variant="caption"
                          component="div"
                          color="text.primary" // Fixed typo: "text.primery" to "text.primary"
                          sx={{ fontSize: "0.9rem", fontWeight: "bold" }}
                        >
                          {`${Math.round(rm.progress || 0)}%`}
                        </Typography>
                      </Box>
                    </Box>
                  </Box>
                </Grid>
              ))}
            </Grid>
          </Section>
        )}
      <Box textAlign="center" mb={4}>
        <Button
          variant="contained"
          sx={{
            padding: "10px 20px",
            width: "200px",
            borderRadius: "20px",
            backgroundColor: "#ee6d4d",
            textTransform: "capitalize",
            color: "#fff",
            "&:hover": { backgroundColor: "#d95b38" },
          }}
          onClick={handleOpenConfirm}
        >
          Delete User
        </Button>
      </Box>
      <Dialog open={confirmOpen} onClose={handleCloseConfirm}>
        <DialogTitle>Confirm Delete</DialogTitle>
        <DialogContent>
          <DialogContentText>
            Are you sure you want to delete this user? This action cannot be
            undone.
          </DialogContentText>
        </DialogContent>
        <DialogActions>
          <Button onClick={handleCloseConfirm}>Cancel</Button>
          <Button color="error" onClick={handleConfirmDelete}>
            Delete
          </Button>
        </DialogActions>
      </Dialog>
      <Snackbar
        open={snackbar.open}
        autoHideDuration={4000}
        onClose={handleCloseSnackbar}
        anchorOrigin={{ vertical: "bottom", horizontal: "center" }}
      >
        <Alert
          onClose={handleCloseSnackbar}
          severity={snackbar.severity}
          sx={{ width: "100%" }}
        >
          {snackbar.message}
        </Alert>
      </Snackbar>
    </Box>
  );
}

function Section({ title, children }) {
  const theme = useTheme();
  return (
    <>
      <Typography
        variant="h6"
        sx={{
          fontWeight: "bold",
          color: "#ee6d4d",
          position: "absolute",
          mt: -2,
          marginLeft: 15,
        }}
      >
        {title}
      </Typography>
      <Card
        sx={{
          border: "2px solid gray",
          borderTop: "none",
          borderRadius: "12px",
          p: 4,
          width: "80%",
          margin: "auto",
          backgroundColor: theme.palette.background.paper,
          boxShadow: "0px 1px 10px rgba(0, 0, 1, 0.1)",
          mb: 4,
        }}
      >
        {children}
      </Card>
    </>
  );
}

function CardContentGrid({ data, userData }) {
  const theme = useTheme();
  return (
    <Box>
      <Grid container spacing={2}>
        {data.map(({ label, field }, index) => (
          <Grid item xs={12} sm={6} key={index}>
            {field ? (
              <Box
                display="flex"
                alignItems="flex-start"
                flexWrap="wrap"
                sx={{ ml: 2 }}
              >
                <Typography
                  sx={{
                    fontSize: "1rem",
                    fontWeight: "bold",
                    color: theme.palette.text.primary,
                    marginRight: "10px",
                    flexShrink: 0,
                  }}
                >
                  {label}
                </Typography>
                <Typography
                  sx={{
                    fontSize: "1rem",
                    fontWeight: 500,
                    color: "gray",
                    wordBreak: "break-all",
                    overflowWrap: "anywhere",
                    flex: "1 1 auto",
                    whiteSpace: "normal",
                  }}
                >
                  {userData[field] || "null"}
                </Typography>
              </Box>
            ) : (
              <Box sx={{ ml: 2 }}>{label}</Box>
            )}
          </Grid>
        ))}
      </Grid>
    </Box>
  );
}