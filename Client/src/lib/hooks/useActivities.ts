import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query"
import agent from '../api/agent'

export const useActivities = () => {
    const queryClient = useQueryClient()
     const {data:activities , isPending} = useQuery({
    queryKey: ['activates'],
    queryFn: async () => {
      const response =await agent.get<Activity[]>('/activities')
      return response.data
    }
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
        await agent.post('/activities', activity)
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
    deleteActivity
  }
}