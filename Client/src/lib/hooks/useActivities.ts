import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query"
import agent from '../api/agent'
import { useLocation } from "react-router"
export const useActivities = (id?: string) => {
    const queryClient = useQueryClient()
    const location  = useLocation()


     const {data:activities , isPending} = useQuery({
    queryKey: ['activates'],
    queryFn: async () => {
      const response =await agent.get<Activity[]>('/activities')
      return response.data
    },
    enabled: !id && location.pathname === 'activities'
  })

  const {data: activity, isLoading: isLoadingActivity} = useQuery({
    queryKey: ['activities', id],
    queryFn: async () => {
        const response =await agent.get<Activity>(`/activities/${id}`)
        return response.data
    },
    enabled: !!id
  })

  const updateActivity = useMutation({
    mutationFn: async (activity: Activity) => {
        await agent.put('/activities', activity)
    },
    onSuccess: async () => {
        await queryClient.invalidateQueries({
            queryKey: ['activates']
        })
    }
  })

  const createActivity = useMutation({
    mutationFn: async (activity: Activity) => {
        const response = await agent.post('/activities', activity)
        return response.data
    },
    onSuccess: async () => {
        await queryClient.invalidateQueries({
            queryKey: ['activates']
        })
    }
  })
  const deleteActivity = useMutation({
    mutationFn: async (id: string) => {
        await agent.post(`/activities/${id}`)
    },
    onSuccess: async () => {
        await queryClient.invalidateQueries({
            queryKey: ['activates']
        })
    }
  })

  return {
    activities,
    isPending,
    updateActivity,
    createActivity,
    deleteActivity,
    activity,
    isLoadingActivity
  }
}