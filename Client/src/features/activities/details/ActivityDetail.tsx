import { Button, Card, CardContent, CardMedia, Typography,CardActions } from "@mui/material"
import { useNavigate, useParams } from "react-router"
import {Link} from 'react-router'
import { useActivities } from "../../../lib/hooks/useActivities"


export default function ActivityDetails() {
    const navigate = useNavigate()
    const {id} = useParams()
    const {activity, isLoadingActivity} = useActivities(id as string);

    if (isLoadingActivity) return <Typography>Loading...</Typography> 

    if(!activity) return <Typography>Activity not found</Typography>

    return (
    <Card sx={{borderRadius: 3}}>
        <CardMedia
            component='img'
            src={`/images/categoryImages/${activity.category}.jpg`}
        />
        <CardContent>
            <Typography variant='h5'>{activity.title}</Typography>
            <Typography variant='subtitle1' fontWeight='light'>{activity.date}</Typography>
            <Typography variant="body1">{activity.description}</Typography>
        </CardContent>
        <CardActions>
            <Button component={Link} to={`/manage/${activity.id}`} color="primary">Edit</Button>
            <Button onClick={() => navigate('/activaties')} color="inherit">Cancel</Button>
        </CardActions>
    </Card>
  )
}
