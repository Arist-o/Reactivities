import { Container, CssBaseline } from "@mui/material"
import NavBar from "./NavBar"
import {Box} from '@mui/material'
import { Outlet } from "react-router"

function App() {




  return (
    <Box sx={{bgcolor: '#eeeeee', minHeight: '100vh'}}>
      <CssBaseline />
      <NavBar/>
      <Container maxWidth='xl' sx={{ mt: { sm: 3 } }}> 
        <Outlet />
      </Container>
    </Box>
  )
}

export default App
