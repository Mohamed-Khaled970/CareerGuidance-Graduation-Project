import * as React from "react";
import { DataGrid } from "@mui/x-data-grid";
import Paper from "@mui/material/Paper";
import { api } from "services/axiosInstance";
import { useEffect, useState } from "react";
import { Box, Button, CircularProgress, Typography } from "@mui/material";
import { useNavigate, useParams } from "react-router-dom";
import Backdrop from "@mui/material/Backdrop";
import Modal from "@mui/material/Modal";
import Fade from "@mui/material/Fade";
import ScheduleInterview from "./ScheduleInterview";

const style = {
  position: "absolute",
  top: "50%",
  left: "50%",
  transform: "translate(-50%, -50%)",
  width: 700,
  bgcolor: "background.paper",
  boxShadow: 24,
  p: 2,
  borderRadius: "15px",
};

export default function ApplicantInterviews() {
  const navigate = useNavigate();
  const [applicantInterviews, setApplicantInterviews] = useState([]);
  const [open, setOpen] = useState(false);
  const [selectedEmail, setSelectedEmail] = useState("");
  const [loading, setLoading] = useState(true);
  const { id } = useParams();

  const fetchApplicantInterviews = async () => {
    setLoading(true);
    try {
      const response = await api.get(`/api/Interview/GetMyApplicants/${id}`);
      const dataWithIds = response.data.map((item, index) => ({
        id: item.interviewId || index + 1,
        interviewId: item.interviewId || index + 1,
        ...item,
      }));
      console.log(response.data);
      setApplicantInterviews(dataWithIds);
    } catch (error) {
      console.error("Error fetching Interviews:", error);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchApplicantInterviews();
  }, []);

  const handleOpen = (email) => {
    setSelectedEmail(email);
    setOpen(true);
  };

  const handleClose = (isInterviewScheduled = false) => {
    setOpen(false);
    setSelectedEmail("");
    if (isInterviewScheduled) {
      fetchApplicantInterviews();
    }
  };

  const handleReject = async (email) => {
    try {
      const response = await api.put(`/api/Interview/RejectApplicant/${id}`, {
        Email: email,
      });
      console.log("Applicant rejected:", response.data);
      fetchApplicantInterviews();
    } catch (error) {
      console.error("Error rejecting applicant:", error);
    }
  };

  const columns = [
    { field: "id", headerName: "ID", width: 70 },
    { field: "userName", headerName: "User Name", width: 200 },
    { field: "email", headerName: "Email", width: 200 },
    {
      field: "cvFilePath",
      headerName: "CV",
      width: 150,
      renderCell: (params) => (
        <a style={{color:"#ee6c4d"}} href={params.value} target="_blank" rel="noopener noreferrer">
          View CV
        </a>
      ),
    },
    {
      field: "accept",
      headerName: "Accept&Schedule",
      width: 150,
      renderCell: (params) => (
        <Button
          variant="outlined"
          color="success"
          onClick={() => handleOpen(params.row.email)}
        >
          Accept
        </Button>
      ),
      sortable: false,
      filterable: false,
    },
    {
      field: "reject",
      headerName: "Reject",
      width: 130,
      renderCell: (params) => (
        <Button
          variant="outlined"
          color="error"
          onClick={() => handleReject(params.row.email)}
        >
          Reject
        </Button>
      ),
      sortable: false,
      filterable: false,
    },
  ];

  return (
    <Box>
      <Typography
        component="h2"
        variant="h5"
        sx={{ mb: 4, textAlign: "center", fontWeight: "bold" }}
      >
        All Applicants
      </Typography>
      {loading ? (
        <Box sx={{ display: "flex", justifyContent: "center", padding: 2 }}>
          <CircularProgress />
        </Box>
      ) : (
        <Paper sx={{ height: 500, width: "90%", margin: "auto" }}>
          <DataGrid
            rows={applicantInterviews}
            columns={columns}
            pageSizeOptions={[5, 10]}
            initialState={{
              pagination: { paginationModel: { page: 0, pageSize: 5 } },
            }}
            sx={{ border: 0 }}
          />
        </Paper>
      )}
      <Modal
        aria-labelledby="transition-modal-title"
        aria-describedby="transition-modal-description"
        open={open}
        closeAfterTransition
        slots={{ backdrop: Backdrop }}
        slotProps={{
          backdrop: {
            timeout: 500,
          },
        }}
      >
        <Fade in={open}>
          <Box sx={style}>
            <ScheduleInterview email={selectedEmail} onClose={handleClose} />
          </Box>
        </Fade>
      </Modal>
    </Box>
  );
}
