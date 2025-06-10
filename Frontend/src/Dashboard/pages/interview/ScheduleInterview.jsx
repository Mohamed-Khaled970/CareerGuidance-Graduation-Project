import React, { useState, useEffect, useMemo } from "react";
import {
  Box,
  Button,
  TextField,
  Typography,
  CircularProgress,
  useTheme,
  Tooltip,
} from "@mui/material";
import { useParams } from "react-router-dom";
import { api } from "services/axiosInstance";

export default function ScheduleInterview({
  email,
  onClose,
  isAccepted = false,
}) {
  const { id } = useParams();
  const [formData, setFormData] = useState({
    email: email || "",
    date: "",
    time: "",
    meetingLink: "",
  });
  const [loading, setLoading] = useState(false);
  const [isInterviewScheduled, setIsInterviewScheduled] = useState(false);
  const [errors, setErrors] = useState({});

  useEffect(() => {
    if (email) {
      setFormData((prev) => ({
        ...prev,
        email,
      }));
    }
  }, [email]);

  const handleChange = (e) => {
    const { name, value } = e.target;
    setFormData((prev) => ({
      ...prev,
      [name]: value,
    }));
    // Clear error for the field being changed
    setErrors((prev) => ({ ...prev, [name]: false }));
  };

  const validateForm = () => {
    const newErrors = {};
    if (!formData.date) newErrors.date = true;
    if (!formData.time) newErrors.time = true;
    if (!formData.meetingLink) newErrors.meetingLink = true;
    // Email is read-only, so no need to validate it here
    return newErrors;
  };

  const handleSend = async (e) => {
    e.preventDefault();
    const newErrors = validateForm();
    if (Object.keys(newErrors).length > 0) {
      setErrors(newErrors);
      return;
    }

    setLoading(true);
    const scheduledDate = new Date(`${formData.date}T${formData.time}:00Z`).toISOString();
    const payload = {
      email: formData.email,
      scheduledDate,
      meetingLink: formData.meetingLink,
    };
    try {
      const response = await api.post(`/api/Interview/ScheduleInterview/${id}`, payload);
      console.log("Interview scheduled:", response.data);
      setIsInterviewScheduled(true);
      isAccepted = true;
      onClose(true); // Pass true to indicate the interview is scheduled
    } catch (error) {
      console.error("Error scheduling interview:", error);
    } finally {
      setLoading(false);
    }
  };

  const handleCancel = () => {
    setFormData({
      email: email || "",
      date: "",
      time: "",
      meetingLink: "",
    });
    onClose(false); // Pass false to indicate no scheduling
  };
  const theme = useTheme();

  return (
    <Box
      sx={{
        maxWidth: 400,
        margin: "0 auto",
        display: "flex",
        flexDirection: "column",
        gap: 2,
        padding: 2,
      }}
    >
      <Typography variant="h5" align="center" gutterBottom>
        Schedule Interview
      </Typography>

      <TextField
        sx={{
          "& .MuiFormLabel-root": { color: theme.palette.text.primary },
          "& .MuiOutlinedInput-root": {
            "&.Mui-focused fieldset": { borderColor: "#ee6c4d" },
          },
        }}
        label="Email"
        name="email"
        type="email"
        value={formData.email}
        InputProps={{ readOnly: true }}
        InputLabelProps={{ shrink: true }}
        required
        fullWidth
      />

      <Tooltip
        title="Please select a date"
        open={errors.date}
        disableHoverListener
        disableFocusListener
        disableTouchListener
      >
        <TextField
          sx={{
            "& .MuiFormLabel-root": { color: theme.palette.text.primary },
            "& .MuiOutlinedInput-root": {
              "&.Mui-focused fieldset": { borderColor: "#ee6c4d" },
            },
          }}
          label="Date"
          name="date"
          type="date"
          value={formData.date}
          onChange={handleChange}
          required
          InputLabelProps={{ shrink: true }}
          fullWidth
          error={errors.date}
          helperText={errors.date && "This field is required"}
        />
      </Tooltip>

      <Tooltip
        title="Please select a time"
        open={errors.time}
        disableHoverListener
        disableFocusListener
        disableTouchListener
      >
        <TextField
          sx={{
            "& .MuiFormLabel-root": { color: theme.palette.text.primary },
            "& .MuiOutlinedInput-root": {
              "&.Mui-focused fieldset": { borderColor: "#ee6c4d" },
            },
          }}
          label="Time"
          name="time"
          type="time"
          value={formData.time}
          onChange={handleChange}
          required
          InputLabelProps={{ shrink: true }}
          fullWidth
          error={errors.time}
          helperText={errors.time && "This field is required"}
        />
      </Tooltip>

      <Tooltip
        title="Please enter a meeting link"
        open={errors.meetingLink}
        disableHoverListener
        disableFocusListener
        disableTouchListener
      >
        <TextField
          sx={{
            "& .MuiFormLabel-root": { color: theme.palette.text.primary },
            "& .MuiOutlinedInput-root": {
              "&.Mui-focused fieldset": { borderColor: "#ee6c4d" },
            },
          }}
          label="Meeting Link"
          name="meetingLink"
          type="url"
          value={formData.meetingLink}
          onChange={handleChange}
          required
          InputLabelProps={{ shrink: true }}
          fullWidth
          error={errors.meetingLink}
          helperText={errors.meetingLink && "This field is required"}
        />
      </Tooltip>

      <Box display="flex" justifyContent="space-between" mt={2}>
        <Button
          variant="contained"
          onClick={handleSend}
          disabled={loading}
          sx={{ backgroundColor: "#ee6c4d", width: "100px", color: "#fff" }}
        >
          {loading ? <CircularProgress size={24} color="inherit" /> : "Send"}
        </Button>
        <Button
          type="button"
          variant="outlined"
          color="error"
          onClick={handleCancel}
        >
          Cancel
        </Button>
      </Box>
    </Box>
  );
}