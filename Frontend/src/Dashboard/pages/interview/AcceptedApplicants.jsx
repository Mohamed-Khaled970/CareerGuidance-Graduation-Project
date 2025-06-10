import * as React from "react";
import { DataGrid } from "@mui/x-data-grid";
import Paper from "@mui/material/Paper";
import { api } from "services/axiosInstance";
import { useEffect, useState } from "react";
import { Box, CircularProgress, Typography } from "@mui/material";

export default function ApplicantInterviews() {
  const [acceptedApplicants, setAcceptedApplicants] = useState([]);
  const [loading, setLoading] = useState(true);

  const columns = [
    { field: "id", headerName: "ID", width: 70 },
    { field: "interviewTitle", headerName: "Interview Title", width: 200 },
    { field: "userName", headerName: "User Name", width: 200 },
    { field: "email", headerName: "Email", width: 200 },
    {
      field: "cv",
      headerName: "CV",
      width: 150,
      renderCell: (params) => (
        <a
          style={{ color: "#ee6c4d" }}
          href={params.value}
          target="_blank"
          rel="noopener noreferrer"
        >
          View CV
        </a>
      ),
    },
    {
      field: "meetingLink",
      headerName: "Meeting Link",
      width: 150,
      renderCell: (params) => (
        <a
          style={{ color: "#ee6c4d" }}
          href={params.value}
          target="_blank"
          rel="noopener noreferrer"
        >
          Join meeting
        </a>
      ),
    },
  ];

  const fetchAcceptedApplicant = () => {
    setLoading(true);
    api
      .get(`/api/Interview/GetAcceptedApplicants`)
      .then((response) => {
        console.log(response.data);
        setAcceptedApplicants(response.data);
      })
      .catch((error) => {
        console.error("Error fetching Interviews:", error);
      })
      .finally(() => {
        setLoading(false);
      });
  };

  useEffect(() => {
    fetchAcceptedApplicant();
  }, []);

  return (
    <Box>
      <Typography
        component="h2"
        variant="h5"
        sx={{ mb: 4, textAlign: "center", fontWeight: "bold" }}
      >
        All Accepted Applicants
      </Typography>
      {loading ? (
        <Box sx={{ display: "flex", justifyContent: "center", padding: 2 }}>
          <CircularProgress />
        </Box>
      ) : (
        <Paper sx={{ height: 500, width: "90%", margin: "auto" }}>
          <DataGrid
            rows={acceptedApplicants}
            columns={columns}
            pageSizeOptions={[5, 10]}
            initialState={{
              pagination: { paginationModel: { page: 0, pageSize: 5 } },
            }}
            sx={{ border: 0 }}
          />
        </Paper>
      )}
    </Box>
  );
}
