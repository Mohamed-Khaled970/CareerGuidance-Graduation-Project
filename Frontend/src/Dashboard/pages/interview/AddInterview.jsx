/*
- File Name: Add.jsx
- Author: Nourhan Khaled
- Date of Creation: [Original Date]
- Versions Information: 1.0.4
- Dependencies: { REACT, MUI, axios, react-router-dom, AuthContext }
- Contributors: Rania Rabie,shrouk Ahmed
- Last Modified Date: 6/8/2025
- Description: Manage interviews with create, update, and delete functionality, with a button that hides when form is shown, Cancel button beside Create/Update, enhanced styling, and smooth transitions, matching the design of AllCarousel.jsx and NewCarousel.jsx.
*/

import React, { useEffect, useState } from "react";
import {
  Box,
  Button,
  FormControl,
  Stack,
  TextField,
  Tooltip,
  useTheme,
  Typography,
  Alert,
  Snackbar,
  Paper,
  IconButton,
  Dialog,
  DialogActions,
  DialogContent,
  DialogContentText,
  DialogTitle,
  MenuItem,
  CircularProgress,
  Fade,
} from "@mui/material";
import { Edit, Delete } from "@mui/icons-material";
import { api } from "../../../services/axiosInstance";
import { useAuth } from "../../../context/AuthContext";

const buttonStyle = {
  display: "block",
  m: "auto",
  color:"#fff",
  textTransform: "capitalize",
  fontSize: "18px",
  backgroundColor: "#ee6c4d",
  boxShadow: "0px 3px 5px rgba(0, 0, 0, 0.2)",
  "&:hover": {
    backgroundColor: "#d65e41",
    boxShadow: "0px 5px 8px rgba(0, 0, 0, 0.3)",
  },
};

const cancelButtonStyle = {
  width: "120px",
  borderColor: "#ee6c4d",
  color: "#ee6c4d",
  textTransform: "capitalize",
  boxShadow: "0px 3px 5px rgba(0, 0, 0, 0.1)",
  "&:hover": {
    borderColor: "#d65e41",
    color: "#d65e41",
    boxShadow: "0px 5px 8px rgba(0, 0, 0, 0.2)",
  },
};

export default function InterviewManagement() {
  const { user, token } = useAuth();
  const theme = useTheme();
  const [interviews, setInterviews] = useState([]);
  const [formData, setFormData] = useState({
    title: "",
    description: "",
    duration: "",
  });
  const [interviewId, setInterviewId] = useState(null);
  const [update, setUpdate] = useState(false);
  const [showForm, setShowForm] = useState(false);
  const [showCreateButton, setShowCreateButton] = useState(true);
  const [loading, setLoading] = useState(false);
  const [errors, setErrors] = useState({
    title: false,
    description: false,
    duration: false,
  });
  const [touched, setTouched] = useState({
    title: false,
    description: false,
    duration: false,
  });
  const [openDialog, setOpenDialog] = useState(false);
  const [openSnackbar, setOpenSnackbar] = useState(false);
  const [errorMessage, setErrorMessage] = useState("");
  const [successMessage, setSuccessMessage] = useState("");

  const durations = [10, 20, 30, 40, 50, 60];

  const validateFields = () => {
    const newErrors = {
      title: formData.title === "",
      description: formData.description === "",
      duration: formData.duration === "",
    };
    setErrors(newErrors);
    return Object.values(newErrors).some((error) => error);
  };

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prev) => ({ ...prev, [name]: value }));
    setErrors((prev) => ({ ...prev, [name]: value === "" }));
    setTouched((prev) => ({ ...prev, [name]: true }));
  };

  const fetchInterviews = async () => {
    try {
      setLoading(true);
      const response = await api.get("/api/Interview/GetMyInterviews", {
        headers: { Authorization: `Bearer ${token}` },
      });
      setInterviews(response.data);
    } catch (error) {
      console.error("Error fetching interviews:", error);
      setErrorMessage(
        error.response?.data?.errors?.[1] ||
          error.response?.data?.message ||
          "Failed to fetch interviews."
      );
      setOpenSnackbar(true);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchInterviews();
  }, [token]);

  const handleAddInterview = async () => {
    setTouched({ title: true, description: true, duration: true });
    if (!validateFields()) {
      setLoading(true);
      try {
        const interviewData = {
          interviewerId: user.id,
          title: formData.title,
          description: formData.description,
          duration: Number(formData.duration),
        };
        await api.post("/api/Interview/AddInterview", interviewData, {
          headers: { Authorization: `Bearer ${token}` },
        });
        setFormData({ title: "", description: "", duration: "" });
        setTouched({ title: false, description: false, duration: false });
        setSuccessMessage("Interview added successfully!");
        setOpenSnackbar(true);
        setShowForm(false);
        setShowCreateButton(true);
        await fetchInterviews();
      } catch (error) {
        console.error("Error adding interview:", error);
        setErrorMessage(
          error.response?.data?.errors?.[1] ||
            error.response?.data?.message ||
            "Failed to add interview."
        );
        setOpenSnackbar(true);
      } finally {
        setLoading(false);
      }
    }
  };

  const handleUpdateInterview = async () => {
    setTouched({ title: true, description: true, duration: true });
    if (!validateFields() && interviewId) {
      setLoading(true);
      try {
        const requestBody = {
          interviewId: interviewId,
          interviewerId: user.id,
          title: formData.title,
          description: formData.description,
          duration: Number(formData.duration),
        };
        await api.put("/api/Interview/UpdateInterview", requestBody, {
          headers: { Authorization: `Bearer ${token}` },
        });
        setFormData({ title: "", description: "", duration: "" });
        setInterviewId(null);
        setUpdate(false);
        setTouched({ title: false, description: false, duration: false });
        setSuccessMessage("Interview updated successfully!");
        setOpenSnackbar(true);
        setShowForm(false);
        setShowCreateButton(true);
        await fetchInterviews();
      } catch (error) {
        console.error("Error updating interview:", error);
        setErrorMessage(
          error.response?.data?.errors?.[1] ||
            error.response?.data?.message ||
            "Failed to update interview."
        );
        setOpenSnackbar(true);
      } finally {
        setLoading(false);
      }
    } else {
      setErrorMessage("Invalid input or missing ID.");
      setOpenSnackbar(true);
    }
  };

  const handleDelete = async () => {
    if (interviewId) {
      try {
        await api.put(`/api/Interview/Delete/${interviewId}`, null, {
          headers: { Authorization: `Bearer ${token}` },
        });
        setInterviews((prev) =>
          prev.filter((interview) => interview.id !== interviewId)
        );
        setSuccessMessage("Interview deleted successfully!");
        setOpenSnackbar(true);
      } catch (error) {
        console.error("Error deleting interview:", error);
        setErrorMessage(
          error.response?.data?.errors?.[1] ||
            error.response?.data?.message ||
            "Failed to delete interview."
        );
        setOpenSnackbar(true);
      }
      handleCloseDialog();
    }
  };

  const handleEditInterview = (interview) => {
    setFormData({
      title: interview.title,
      description: interview.description,
      duration: interview.duration,
    });
    setInterviewId(interview.id);
    setUpdate(true);
    setShowForm(true);
    setShowCreateButton(false);
  };

  const handleOpenDialog = (id) => {
    setInterviewId(id);
    setOpenDialog(true);
  };

  const handleCloseDialog = () => {
    setOpenDialog(false);
    setInterviewId(null);
  };

  const handleCloseSnackbar = () => {
    setOpenSnackbar(false);
    setErrorMessage("");
    setSuccessMessage("");
  };

  const handleCancelClick = () => {
    setUpdate(false);
    setFormData({ title: "", description: "", duration: "" });
    setInterviewId(null);
    setTouched({ title: false, description: false, duration: false });
    setShowForm(false);
    setShowCreateButton(true);
  };

  const handleCreateNewInterview = () => {
    setShowForm(true);
    setShowCreateButton(false);
    setUpdate(false);
    setFormData({ title: "", description: "", duration: "" });
    setTouched({ title: false, description: false, duration: false });
  };

  return (
    <Box sx={{ width: "80%", m: "auto", mt: 2 }}>
      <Typography
        component="h2"
        variant="h5"
        sx={{ my: 2, textAlign: "center", fontWeight: "bold" }}
      >
        All Interviews
      </Typography>
      {loading ? (
        <Box display="flex" justifyContent="center" p={4}>
          <CircularProgress />
        </Box>
      ) : (
        <Stack spacing={2} alignItems="center">
          {interviews.length > 0 ? (
            interviews.map((interview) => (
              <Paper
                key={interview.id}
                elevation={2}
                sx={{
                  display: "flex",
                  alignItems: "center",
                  width: { xs: "90%", md: "50%" },
                  py: 1,
                  px: 2,
                  backgroundColor:
                    theme.palette.mode === "dark" ? "#333" : "#fff",
                }}
              >
                <Typography sx={{ flexGrow: 1 }}>{interview.title}</Typography>
                <Tooltip title="Edit interview">
                  <IconButton
                    aria-label="edit"
                    onClick={() => handleEditInterview(interview)}
                  >
                    <Edit sx={{ color: theme.palette.text.secondary }} />
                  </IconButton>
                </Tooltip>
                <Tooltip title="Delete interview">
                  <IconButton
                    aria-label="delete"
                    onClick={() => handleOpenDialog(interview.id)}
                  >
                    <Delete sx={{ color: theme.palette.error.light }} />
                  </IconButton>
                </Tooltip>
              </Paper>
            ))
          ) : (
            <Typography align="center" color="textSecondary">
              No interviews found
            </Typography>
          )}
        </Stack>
      )}

      {showCreateButton && (
        <Fade in={showCreateButton} timeout={300}>
          <Button
            variant="contained"
            sx={{ ...buttonStyle, mt: 4, mb: 2 }}
            onClick={handleCreateNewInterview}
          >
            Create New Interview
          </Button>
        </Fade>
      )}

      <Fade in={showForm} timeout={300}>
        <Box
          sx={{
            my: 2,
            display: "flex",
            flexDirection: "column",
            alignItems: "center",
            transition: "opacity 0.3s ease-in-out",
          }}
        >
          <Typography
            component="h3"
            variant="h6"
            sx={{ my: 2, textAlign: "center", fontWeight: "medium" }}
          >
            {update ? "Edit Interview" : "Add New Interview"}
          </Typography>
          <FormControl
            variant="outlined"
            error={errors.title}
            sx={{ mb: 2, width: "100%", maxWidth: 350 }}
          >
            <Typography
              component="label"
              sx={{ mb: 1, fontSize: "16px", fontWeight: "medium" }}
            >
              Title
            </Typography>
            <Tooltip
              title={
                touched.title && errors.title ? "This field is required." : ""
              }
              arrow
              open={touched.title && errors.title}
              disableHoverListener={!errors.title}
            >
              <TextField
                name="title"
                value={formData.title}
                onChange={handleChange}
                autoComplete="off"
                error={errors.title}
                placeholder="Enter interview title"
                sx={{
                  "& .MuiOutlinedInput-root": {
                    backgroundColor:
                      theme.palette.mode === "dark" ? "#262626" : "#D9D9D9",
                    color: theme.palette.text.primary,
                    borderRadius: "10px",
                    height: "45px",
                    "& fieldset": {
                      borderColor:
                        theme.palette.mode === "dark" ? "#555" : "#ccc",
                    },
                  },
                }}
              />
            </Tooltip>
          </FormControl>

          <FormControl
            variant="outlined"
            error={errors.description}
            sx={{ mb: 2, width: "100%", maxWidth: 350 }}
          >
            <Typography
              component="label"
              sx={{ mb: 1, fontSize: "16px", fontWeight: "medium" }}
            >
              Description
            </Typography>
            <Tooltip
              title={
                touched.description && errors.description
                  ? "This field is required."
                  : ""
              }
              arrow
              open={touched.description && errors.description}
              disableHoverListener={!errors.description}
            >
              <TextField
                name="description"
                multiline
                minRows={4}
                value={formData.description}
                onChange={handleChange}
                autoComplete="off"
                error={errors.description}
                placeholder="Enter interview description"
                sx={{
                  "& .MuiOutlinedInput-root": {
                    backgroundColor:
                      theme.palette.mode === "dark" ? "#262626" : "#D9D9D9",
                    color: theme.palette.text.primary,
                    borderRadius: "10px",
                    "& fieldset": {
                      borderColor:
                        theme.palette.mode === "dark" ? "#555" : "#ccc",
                    },
                  },
                }}
              />
            </Tooltip>
          </FormControl>

          <FormControl
            variant="outlined"
            error={errors.duration}
            sx={{ mb: 2, width: "100%", maxWidth: 350 }}
          >
            <Typography
              component="label"
              sx={{ mb: 1, fontSize: "16px", fontWeight: "medium" }}
            >
              Duration (minutes)
            </Typography>
            <Tooltip
              title={
                touched.duration && errors.duration
                  ? "This field is required."
                  : ""
              }
              arrow
              open={touched.duration && errors.duration}
              disableHoverListener={!errors.duration}
            >
              <TextField
                name="duration"
                select
                value={formData.duration}
                onChange={handleChange}
                autoComplete="off"
                error={errors.duration}
                placeholder="Select duration"
                sx={{
                  "& .MuiOutlinedInput-root": {
                    backgroundColor:
                      theme.palette.mode === "dark" ? "#262626" : "#D9D9D9",
                    color: theme.palette.text.primary,
                    borderRadius: "10px",
                    height: "45px",
                    "& fieldset": {
                      borderColor:
                        theme.palette.mode === "dark" ? "#555" : "#ccc",
                    },
                  },
                }}
              >
                <MenuItem value="">Select Duration</MenuItem>
                {durations.map((dur) => (
                  <MenuItem key={dur} value={dur}>
                    {dur} min
                  </MenuItem>
                ))}
              </TextField>
            </Tooltip>
          </FormControl>

          <Box sx={{ display: "flex", gap: 2, my: 2 }}>
            <Button
              variant="contained"
              onClick={update ? handleUpdateInterview : handleAddInterview}
              disabled={loading}
              sx={buttonStyle}
            >
              {loading ? (
                <CircularProgress size={20} />
              ) : update ? (
                "Update"
              ) : (
                "Create"
              )}
            </Button>
            <Button
              variant="outlined"
              onClick={handleCancelClick}
              sx={cancelButtonStyle}
            >
              Cancel
            </Button>
          </Box>
        </Box>
      </Fade>

      <Dialog open={openDialog} onClose={handleCloseDialog}>
        <DialogTitle>Confirm Delete</DialogTitle>
        <DialogContent>
          <DialogContentText>
            Are you sure you want to delete this interview?
          </DialogContentText>
        </DialogContent>
        <DialogActions>
          <Button
            onClick={handleCloseDialog}
            sx={{ color: theme.palette.text.primary }}
          >
            Cancel
          </Button>
          <Button
            onClick={handleDelete}
            autoFocus
            sx={{ color: theme.palette.error.main }}
          >
            Delete
          </Button>
        </DialogActions>
      </Dialog>

      <Snackbar
        open={openSnackbar}
        autoHideDuration={6000}
        onClose={handleCloseSnackbar}
        anchorOrigin={{ vertical: "top", horizontal: "center" }}
      >
        <Alert
          onClose={handleCloseSnackbar}
          severity={errorMessage ? "error" : "success"}
          sx={{ width: "100%" }}
        >
          {errorMessage || successMessage}
        </Alert>
      </Snackbar>
    </Box>
  );
}
