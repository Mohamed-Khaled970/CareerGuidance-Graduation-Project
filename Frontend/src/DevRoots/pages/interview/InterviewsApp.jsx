import React, { useState, useRef, useEffect } from "react";
import {
  Box,
  Typography,
  Button,
  CircularProgress,
  Snackbar,
  Alert,
  Grid,
  Card,
  CardContent,
  CardActions,
  Dialog,
  DialogTitle,
  DialogContent,
  DialogActions,
  Avatar,
  useTheme,
  Chip,
  IconButton,
} from "@mui/material";
import { GitHub, LinkedIn, Person } from "@mui/icons-material";
import { api } from "../../../services/axiosInstance";
import { useAuth } from "../../../context/AuthContext";

// Main component for displaying and managing interviews
const InterviewApp = () => {
  // States
  const [interviews, setInterviews] = useState([]); // List of interviews
  const [loading, setLoading] = useState(true); // Loading state
  const [error, setError] = useState(null); // Error messages
  const [success, setSuccess] = useState(null); // Success messages
  const [openSnackbar, setOpenSnackbar] = useState(false); // Control for Snackbar
  const [showUploadPopup, setShowUploadPopup] = useState(false); // Control for CV upload popup
  const [currentUploadInterviewId, setCurrentUploadInterviewId] =
    useState(null); // Current interview ID for upload
  const [fileMap, setFileMap] = useState({}); // Selected files
  const [profileInterview, setProfileInterview] = useState(null); // Interviewer profile details
  const fileInputRef = useRef(null); // Reference for file input element
  const { token, user } = useAuth(); // Token and user info
  const theme = useTheme(); // Theme for dark/light mode support

  // Fetch interviews on page load
  useEffect(() => {
    const fetchInterviews = async () => {
      try {
        setLoading(true);
        // Fetch interviews from API
        const response = await api.get("/api/Interview/GetAllInterviews", {
          headers: { Authorization: `Bearer ${token}` },
        });
        if (!response.data) {
          throw new Error("No data returned from API");
        }
        // Get application status from localStorage
        const storedApplied = JSON.parse(
          localStorage.getItem("appliedInterviews") || "{}"
        );
        // Transform data into required format
        const interviewsData = Array.isArray(response.data)
          ? response.data.map((interview) => ({
              id: interview.interviewId || "N/A",
              title: interview.title || "Untitled Interview",
              description: interview.description || "No description provided",
              interviewerId: interview.interviewerId || "N/A",
              interviewer: {
                name: "Unknown Interviewer", // Default value until name is fetched
                email: interview.interviewerEmail || "N/A",
                imageUrl:
                  interview.interviewerImage ||
                  "https://via.placeholder.com/120",
                about: interview.interviewerAbout || "No description available",
              },
              hasApplied:
                storedApplied[interview.interviewId] || // If exists in storedApplied
                interview.hasApplied || // If exists in response
                false, // Default value
            }))
          : [];

        // Fetch interviewer names from separate endpoint
        const uniqueInterviewerIds = [
          // @ts-ignore
          ...new Set(
            interviewsData
              .map((i) => i.interviewerId)
              .filter((id) => id !== "N/A")
          ),
        ];
        const interviewerProfiles = await Promise.all(
          uniqueInterviewerIds.map(async (id) => {
            try {
              const profileResponse = await api.get(
                `/api/Interview/profile/${id}`,
                {
                  headers: { Authorization: `Bearer ${token}` },
                }
              );
              return {
                id,
                name: profileResponse.data.name || "Unknown Interviewer",
              };
            } catch (err) {
              console.error(
                `Failed to fetch profile for interviewer ${id}`,
                err
              );
              return { id, name: "Unknown Interviewer" };
            }
          })
        );

        // Update interviewer names in interviewsData
        const interviewerNameMap = interviewerProfiles.reduce(
          (acc, { id, name }) => {
            acc[id] = name;
            return acc;
          },
          {}
        );

        const updatedInterviews = interviewsData.map((interview) => ({
          ...interview,
          interviewer: {
            ...interview.interviewer,
            name:
              interviewerNameMap[interview.interviewerId] ||
              "Unknown Interviewer",
          },
        }));

        setInterviews(updatedInterviews);
        // Show message if no interviews available
        if (updatedInterviews.length === 0) {
          setError("No interviews available at the moment");
          setOpenSnackbar(true);
        }
      } catch (err) {
        handleApiError("Failed to fetch interviews", err);
      } finally {
        setLoading(false);
      }
    };
    fetchInterviews();
  }, [token]);

  // Auto-open file selection when upload popup opens
  useEffect(() => {
    if (showUploadPopup) {
      setTimeout(() => fileInputRef.current?.click(), 100);
    }
  }, [showUploadPopup]);

  // Fetch interviewer profile from separate endpoint
  const fetchInterviewerProfile = async (interviewerId) => {
    try {
      const response = await api.get(
        `/api/Interview/profile/${interviewerId}`,
        {
          headers: { Authorization: `Bearer ${token}` },
        }
      );
      return response.data;
    } catch (err) {
      handleApiError(
        `Failed to fetch profile for interviewer ${interviewerId}`,
        err
      );
      return null;
    }
  };

  // Handle API errors
  const handleApiError = (defaultMessage, err) => {
    console.error("API Error:", defaultMessage, err);
    const errorMessage =
      err.response?.data?.message || err.message || defaultMessage;
    setError(errorMessage);
    setOpenSnackbar(true);
  };

  // Handle success messages
  const handleSuccess = (message) => {
    setSuccess(message);
    setOpenSnackbar(true);
  };

  // Open CV upload popup
  const openUploadPopup = (interviewId) => {
    if (!interviewId || interviewId === "N/A") {
      setError("Invalid interview ID");
      setOpenSnackbar(true);
      return;
    }
    setCurrentUploadInterviewId(interviewId);
    setShowUploadPopup(true);
  };

  // Close CV upload popup
  const closeUploadPopup = () => {
    setShowUploadPopup(false);
    setCurrentUploadInterviewId(null);
    setFileMap((prev) => {
      const updated = { ...prev };
      delete updated[currentUploadInterviewId];
      return updated;
    });
  };

  // Handle file selection for CV
  const handleFileSelect = (e) => {
    const file = e.target.files[0];
    if (!file) {
      setError("Please select a file");
      setOpenSnackbar(true);
      return;
    }
    const allowedTypes = ["application/pdf"];
    if (!allowedTypes.includes(file.type)) {
      setError("Please select a PDF file");
      setOpenSnackbar(true);
      return;
    }
    if (file.size > 1 * 1024 * 1024) {
      setError("File size exceeds 1MB limit");
      setOpenSnackbar(true);
      return;
    }
    const fileNameRegex = /^[a-zA-Z0-9_-]+\.pdf$/;
    if (!fileNameRegex.test(file.name)) {
      setError("Invalid file name");
      setOpenSnackbar(true);
      return;
    }
    setFileMap((prev) => ({ ...prev, [currentUploadInterviewId]: file }));
  };

  // Upload CV to API
  const handleUpload = async () => {
    const file = fileMap[currentUploadInterviewId];
    if (!file) {
      setError("Please select a valid PDF file");
      setOpenSnackbar(true);
      return;
    }
    if (!currentUploadInterviewId || !user?.id) {
      setError("Invalid data");
      setOpenSnackbar(true);
      return;
    }
    try {
      const formData = new FormData();
      formData.append("CVFile", file);
      formData.append("InterviewId", currentUploadInterviewId);
      formData.append("UserId", user.id);
      const response = await api.post("/api/Interview/Apply", formData, {
        headers: {
          Authorization: `Bearer ${token}`,
          "Content-Type": "multipart/form-data",
        },
      });
      // Update application status in localStorage
      const storedApplied = JSON.parse(
        localStorage.getItem("appliedInterviews") || "{}"
      );
      storedApplied[currentUploadInterviewId] = true;
      localStorage.setItem("appliedInterviews", JSON.stringify(storedApplied));
      // Update interview status
      setInterviews(
        interviews.map((i) =>
          i.id === currentUploadInterviewId ? { ...i, hasApplied: true } : i
        )
      );
      handleSuccess("CV uploaded successfully");
      closeUploadPopup();
    } catch (err) {
      handleApiError("Failed to upload CV", err);
    }
  };

  // Open new file selection
  const handleChangeFile = () => fileInputRef.current?.click();

  // Delete selected file
  const handleDeleteFile = () => {
    setFileMap((prev) => {
      const updated = { ...prev };
      delete updated[currentUploadInterviewId];
      return updated;
    });
    fileInputRef.current?.click();
  };

  // Open interviewer profile popup
  const openProfilePopup = async (interview) => {
    const profile = await fetchInterviewerProfile(interview.interviewerId);
    if (profile) {
      setProfileInterview({
        ...interview,
        interviewer: {
          name: profile.name || "Unknown Interviewer",
          email: profile.email || "N/A",
          imageUrl: profile.imageUrl || "https://via.placeholder.com/120",
          about: profile.about || "No description available",
          github: profile.github || null,
          linkedIn: profile.linkedIn || null,
        },
      });
    }
  };

  // Close interviewer profile popup
  const closeProfilePopup = () => setProfileInterview(null);

  // Close Snackbar
  const handleCloseSnackbar = () => {
    setOpenSnackbar(false);
    setError(null);
    setSuccess(null);
  };

  // Display loading state
  if (loading) {
    return (
      <Box
        sx={{
          display: "flex",
          justifyContent: "center",
          p: 4,
          bgcolor: theme.palette.background.default,
        }}
      >
        <CircularProgress color="primary" />
      </Box>
    );
  }

  return (
    <Box
      sx={{
        maxWidth: "1200px",
        mx: "auto",
        p: 4,
        bgcolor: theme.palette.background.default,
        color: theme.palette.text.primary,
      }}
    >
      <Typography
        variant="h4"
        align="center"
        fontWeight="bold"
        sx={{
          mb: 5,
          color: theme.palette.text.primary,
          textShadow: "2px 2px 4px rgba(0,0,0,0.1)",
          letterSpacing: "1px",
        }}
      >
        Available Interviews
      </Typography>
      {interviews.length > 0 ? (
        <Grid container spacing={3}>
          {interviews.map((interview) => (
            <Grid item xs={12} sm={6} md={4} key={interview.id}>
              <Card
                sx={{
                  bgcolor: theme.palette.background.paper,
                  color: theme.palette.text.primary,
                  borderRadius: "12px",
                  boxShadow: "0 4px 20px rgba(0,0,0,0.1)",
                  transition: "transform 0.3s ease, box-shadow 0.3s ease",
                  "&:hover": {
                    transform: "translateY(-5px)",
                    boxShadow: "0 8px 30px rgba(0,0,0,0.15)",
                  },
                  overflow: "hidden",
                }}
              >
                <CardContent sx={{ p: 3 }}>
                  <Box
                    sx={{
                      display: "flex",
                      justifyContent: "space-between",
                      alignItems: "center",
                      mb: 2,
                    }}
                  >
                    <Typography
                      variant="h6"
                      sx={{
                        // @ts-ignore
                        color: theme.palette.text.main,
                        fontWeight: "bold",
                        fontSize: "1.2rem",
                        lineHeight: 1.4,
                      }}
                    >
                      {interview.title}
                    </Typography>
                    {interview.hasApplied && (
                      <Chip
                        label="Applied"
                        size="small"
                        sx={{
                          bgcolor: theme.palette.success.light,
                          color: theme.palette.success.contrastText,
                          fontWeight: "medium",
                          borderRadius: "8px",
                        }}
                      />
                    )}
                  </Box>
                  <Box
                    sx={{
                      display: "flex",
                      alignItems: "center",
                      mb: 2,
                      gap: 1,
                    }}
                  >
                    {/* <Person
                      sx={{
                        color: theme.palette.text.main,
                        fontSize: "1.2rem",
                      }}
                    /> */}
                    <Typography
                      variant="body1" // Change to body1 for larger size
                      sx={{
                        color: theme.palette.text.primary,
                        fontWeight: "medium",
                        fontStyle: "italic",
                        fontSize: "1rem", // Manual size increase (optional)
                      }}
                    >
                      Interviewer: {interview.interviewer.name}
                    </Typography>
                  </Box>
                  <Typography
                    variant="body2"
                    sx={{
                      color: theme.palette.text.secondary,
                      mb: 2,
                      lineHeight: 1.6,
                      fontSize: "0.9rem",
                      display: "-webkit-box",
                      WebkitBoxOrient: "vertical",
                      WebkitLineClamp: 3,
                      overflow: "hidden",
                      textOverflow: "ellipsis",
                    }}
                  >
                    Details: {interview.description}
                  </Typography>
                </CardContent>
                <CardActions
                  sx={{
                    p: 2,
                    justifyContent: "space-between",
                    bgcolor: "rgba(0,0,0,0.02)",
                  }}
                >
                  <Button
                    variant="outlined"
                    size="medium"
                    onClick={() => openProfilePopup(interview)}
                    sx={{
                      // @ts-ignore
                      color: theme.palette.text.main,
                      // @ts-ignore
                      borderColor: theme.palette.text.main,
                      borderRadius: "8px",
                      textTransform: "none",
                      fontWeight: "medium",
                      transition: "all 0.2s ease",
                      "&:hover": {
                        transform: "scale(1.05)",
                      },
                    }}
                  >
                    View Details
                  </Button>
                  {interview.hasApplied ? (
                    <Button
                      variant="contained"
                      size="medium"
                      sx={{
                        bgcolor: theme.palette.success.main,
                        color: theme.palette.success.contrastText,
                        borderRadius: "8px",
                        textTransform: "none",
                        fontWeight: "bold",
                        pointerEvents: "none",
                        transition: "all 0.2s ease",
                      }}
                    >
                      ✓ Applied
                    </Button>
                  ) : (
                    <Button
                      variant="contained"
                      size="medium"
                      onClick={() => openUploadPopup(interview.id)}
                      sx={{
                        bgcolor: theme.palette.text.main,
                        color: theme.palette.background.paper,
                        borderRadius: "8px",
                        textTransform: "none",
                        fontWeight: "bold",
                        transition: "all 0.2s ease",
                        "&:hover": {
                          bgcolor: theme.palette.text.main + "D0",
                          transform: "scale(1.05)",
                        },
                      }}
                    >
                      Apply Now
                    </Button>
                  )}
                </CardActions>
              </Card>
            </Grid>
          ))}
        </Grid>
      ) : (
        <Typography
          variant="body1"
          align="center"
          sx={{
            color: theme.palette.text.primary,
            py: 6,
            fontSize: "1.2rem",
            fontWeight: "medium",
          }}
        >
          No interviews available at the moment
        </Typography>
      )}

      <Dialog
        open={showUploadPopup}
        onClose={closeUploadPopup}
        maxWidth="sm"
        fullWidth
        sx={{
          "& .MuiDialog-paper": {
            borderRadius: "16px",
            boxShadow: "0 10px 30px rgba(0,0,0,0.2)",
            bgcolor: theme.palette.background.paper,
            border: `2px solid ${theme.palette.text.main}`,
          },
        }}
      >
        <DialogTitle
          sx={{
            bgcolor: theme.palette.text.main,
            color: theme.palette.background.paper,
            fontWeight: "bold",
            textAlign: "center",
            py: 2.5,
            borderTopLeftRadius: "14px",
            borderTopRightRadius: "14px",
          }}
        >
          Upload Your CV
        </DialogTitle>
        <DialogContent
          sx={{ p: 4, textAlign: "center", color: theme.palette.text.primary }}
        >
          <input
            type="file"
            ref={fileInputRef}
            accept="application/pdf"
            style={{ display: "none" }}
            onChange={handleFileSelect}
          />
          {fileMap[currentUploadInterviewId] ? (
            <Typography
              sx={{
                color: theme.palette.text.primary,
                mb: 2,
                fontWeight: "medium",
                bgcolor: theme.palette.background.default,
                p: 2,
                borderRadius: "8px",
              }}
            >
              Selected: {fileMap[currentUploadInterviewId].name}
            </Typography>
          ) : (
            <Typography
              sx={{
                color: theme.palette.text.secondary,
                mb: 2,
                fontStyle: "italic",
              }}
            >
              No file chosen yet
            </Typography>
          )}
          <Box
            sx={{
              display: "flex",
              gap: 2,
              mt: 3,
              flexWrap: "wrap",
              justifyContent: "center",
            }}
          >
            <Button
              variant="contained"
              onClick={handleUpload}
              disabled={!fileMap[currentUploadInterviewId]}
              sx={{
                flex: "1 1 30%",
                bgcolor: theme.palette.success.main,
                color: theme.palette.background.paper,
                borderRadius: "8px",
                textTransform: "none",
                fontWeight: "bold",
                transition: "all 0.2s ease",
                "&:hover": {
                  bgcolor: theme.palette.success.dark,
                  transform: "scale(1.05)",
                },
                "&:disabled": {
                  bgcolor: theme.palette.grey[400],
                },
              }}
            >
              Upload CV
            </Button>
            {fileMap[currentUploadInterviewId] && (
              <Button
                variant="outlined"
                onClick={handleDeleteFile}
                sx={{
                  flex: "1 1 30%",
                  borderColor: theme.palette.error.main,
                  color: theme.palette.error.main,
                  borderRadius: "6px",
                  textTransform: "none",
                  transition: "all 0.2s ease",
                  "&:hover": {
                    borderColor: theme.palette.error.dark,
                    color: theme.palette.error.dark,
                    bgcolor: "rgba(211, 47, 47, 0.05)",
                    transform: "scale(1.05)",
                  },
                }}
              >
                Delete File
              </Button>
            )}
            <Button
              variant="outlined"
              onClick={handleChangeFile}
              sx={{
                flex: "1 1 30%",
                borderColor: theme.palette.text.main,
                color: theme.palette.text.main,
                borderRadius: "6px",
                textTransform: "none",
                transition: "all 0.2s ease",
                "&:hover": {
                  borderColor: theme.palette.text.main + "D0",
                  color: theme.palette.text.main,
                  bgcolor: theme.palette.text.main + "20",
                  transform: "scale(1.05)",
                },
              }}
            >
              Change File
            </Button>
          </Box>
        </DialogContent>
        <DialogActions sx={{ p: 2, justifyContent: "center" }}>
          <Button
            onClick={closeUploadPopup}
            sx={{
              color: theme.palette.text.primary,
              fontWeight: "bold",
              textTransform: "none",
              transition: "all 0.2s ease",
              "&:hover": {
                color: theme.palette.error.main,
                transform: "scale(1.05)",
              },
            }}
          >
            Cancel
          </Button>
        </DialogActions>
      </Dialog>

      <Dialog
        open={!!profileInterview}
        onClose={closeProfilePopup}
        maxWidth="sm"
        fullWidth
        sx={{
          "& .MuiDialog-paper": {
            borderRadius: "16px",
            boxShadow: "0 10px 30px rgba(0,0,0,0.2)",
            bgcolor: theme.palette.background.paper,
            border: `2px solid ${theme.palette.text.main}`,
          },
        }}
      >
        <DialogTitle
          sx={{
            bgcolor: theme.palette.background.paper,
            color: theme.palette.text.primary,
            fontWeight: "bold",
            fontSize: "1.8rem",
            textAlign: "center",
            py: 3,
            borderTopLeftRadius: "14px",
            borderTopRightRadius: "14px",
            letterSpacing: "0.5px",
          }}
        >
          {profileInterview?.interviewer?.name || "Unknown Interviewer"}
        </DialogTitle>
        <DialogContent
          sx={{
            p: 4,
            display: "flex",
            flexDirection: "column",
            alignItems: "center",
            gap: 3,
            color: theme.palette.text.primary,
            bgcolor: theme.palette.background.paper,
          }}
        >
          <Avatar
            src={profileInterview?.interviewer?.imageUrl}
            alt={profileInterview?.interviewer?.name}
            sx={{
              width: 120,
              height: 120,
              border: `4px solid ${theme.palette.text.main}`,
              boxShadow: "0 4px 10px rgba(0,0,0,0.2)",
              transition: "transform 0.3s ease",
              "&:hover": {
                transform: "scale(1.05)",
              },
            }}
          />
          <Box sx={{ textAlign: "center", width: "100%" }}>
            <Typography
              variant="h6"
              sx={{
                color: theme.palette.text.main,
                fontWeight: "bold",
                mb: 1,
                fontSize: "1.2rem",
              }}
            >
              Interviewer
            </Typography>
            <Typography
              sx={{
                color: theme.palette.text.primary,
                mb: 1,
                fontSize: "1rem",
                fontWeight: "medium",
              }}
            >
              Email: {profileInterview?.interviewer?.email || "N/A"}
            </Typography>
            <Typography
              variant="body1"
              sx={{
                color: theme.palette.text.primary,
                lineHeight: "1.7",
                mb: 2,
                fontSize: "0.95rem",
                maxWidth: "400px",
                mx: "auto",
              }}
            >
              {profileInterview?.interviewer?.about ||
                "No description available"}
            </Typography>
            <Box sx={{ display: "flex", justifyContent: "center", gap: 2 }}>
              {profileInterview?.interviewer?.github && (
                <IconButton
                  component="a"
                  href={profileInterview.interviewer.github}
                  target="_blank"
                  sx={{
                    color: theme.palette.text.primary,
                    transition: "all 0.3s ease",
                    "&:hover": {
                      transform: "scale(1.2)",
                    },
                  }}
                  aria-label="GitHub Profile"
                >
                  <GitHub sx={{ fontSize: "2rem" }} />
                </IconButton>
              )}
              {profileInterview?.interviewer?.linkedIn && (
                <IconButton
                  component="a"
                  href={profileInterview.interviewer.linkedIn}
                  target="_blank"
                  sx={{
                    color: theme.palette.text.primary,
                    transition: "all 0.3s ease",
                    "&:hover": {
                      transform: "scale(1.2)",
                    },
                  }}
                  aria-label="LinkedIn Profile"
                >
                  <LinkedIn sx={{ fontSize: "2rem" }} />
                </IconButton>
              )}
            </Box>
          </Box>
        </DialogContent>
        <DialogActions sx={{ p: 2, justifyContent: "center" }}>
          <Button
            variant="outlined"
            size="medium"
            onClick={closeProfilePopup}
            sx={{
              // @ts-ignore
              color: theme.palette.text.primary,
              // @ts-ignore
              borderColor: theme.palette.text.main,
              borderRadius: "8px",
              textTransform: "none",
              fontWeight: "medium",
              transition: "all 0.2s ease",
              "&:hover": {
                transform: "scale(1.05)",
              },
            }}
          >
            Close
          </Button>
        </DialogActions>
      </Dialog>

      <Snackbar
        open={openSnackbar}
        autoHideDuration={3000}
        onClose={handleCloseSnackbar}
        anchorOrigin={{ vertical: "top", horizontal: "center" }}
      >
        <Alert
          onClose={handleCloseSnackbar}
          severity={error ? "error" : "success"}
          sx={{
            width: "100%",
            bgcolor: error
              ? theme.palette.error.light
              : theme.palette.success.light,
            color: theme.palette.text.primary,
            fontWeight: "medium",
            borderRadius: "8px",
            boxShadow: "0 4px 10px rgba(0,0,0,0.1)",
          }}
        >
          {error || success}
        </Alert>
      </Snackbar>
    </Box>
  );
};

export default InterviewApp;