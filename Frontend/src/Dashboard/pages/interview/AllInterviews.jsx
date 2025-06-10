import React from "react";
import Paper from "@mui/material/Paper";
import { api } from "services/axiosInstance";
import { useEffect, useState } from "react";
import {
  Box,
  Button,
  Stack,
  Typography,
  CircularProgress,
  useTheme,
} from "@mui/material";
import { useNavigate } from "react-router-dom";

export default function AllInterviews() {
  const [allInterviews, setAllInterviews] = useState([]);
  const [loading, setLoading] = useState(true);
  const navigate = useNavigate();
  const theme = useTheme();

  const fetchAllInterviews = () => {
    setLoading(true);
    api
      .get("/api/Interview/GetMyInterviews")
      .then((response) => {
        setAllInterviews(response.data);
        console.log(response.data);
      })
      .catch((error) => {
        console.error("Error fetching Interviews:", error);
      })
      .finally(() => {
        setLoading(false);
      });
  };

  useEffect(() => {
    fetchAllInterviews();
  }, []);

  const handleViewApplicantsClick = (id) => {
    navigate(`/dashboard/interview/ApplicantInterviews/${id}`);
  };
  const handleViewAcceptedApplicantsClick = () => {
    navigate(`/dashboard/interview/acceptedInterviews`);
  };

  if (loading) {
    return (
      <Box textAlign="center" mt={4}>
        <CircularProgress />
      </Box>
    );
  }

  return (
    <Box>
      <Typography
        component="h2"
        variant="h5"
        sx={{ mb: 4, textAlign: "center", fontWeight: "bold" }}
      >
        All Interviews
      </Typography>
      <Stack spacing={2} alignItems={"center"}>
        {allInterviews.map((interview) => (
          <Paper
            key={interview.id}
            elevation={2}
            sx={{
              display: "flex",
              alignItems: "center",
              justifyContent: "space-between",
              width: { xs: "90%", md: "50%" },
              py: 1,
              px: 2,
            }}
          >
            <Typography width={"30%"}>{interview.title}</Typography>
            <Button
              onClick={() => handleViewApplicantsClick(interview.id)}
              sx={{
                textTransform: "capitalize",
                color: theme.palette.text.main,
              }}
            >
              View Applicants
            </Button>

            <Button
              onClick={() => handleViewAcceptedApplicantsClick()}
              sx={{
                textTransform: "capitalize",
                color: theme.palette.text.main,
              }}
            >
              View Accepted Applicants
            </Button>
          </Paper>
        ))}
      </Stack>
    </Box>
  );
}
