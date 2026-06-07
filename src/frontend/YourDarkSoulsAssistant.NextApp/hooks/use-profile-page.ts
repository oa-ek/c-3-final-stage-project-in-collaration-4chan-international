import { ChangeEvent, useCallback, useEffect, useRef, useState } from 'react'
import { useRouter } from 'next/navigation'
import { useAuth } from '@/contexts/auth-context'
import { tokenStorage } from '@/lib/api-client'
import { UpdateUserRequestDTO } from '@/types/dto/users'

type AdminTab = 'dashboard' | 'users' | 'equipment'

export function useProfilePage() {
  const router = useRouter()
  const { user, isLoading, isAdmin, logout, updateProfile } = useAuth()

  const [showAdminPanel, setShowAdminPanel] = useState(false)
  const [adminTab, setAdminTab] = useState<AdminTab>('dashboard')
  const [isUploadingAvatar, setIsUploadingAvatar] = useState(false)
  const [avatarCacheBuster, setAvatarCacheBuster] = useState(Date.now())
  const fileInputRef = useRef<HTMLInputElement>(null)
  const [isEditing, setIsEditing] = useState(false)
  const [isSaving, setIsSaving] = useState(false)
  const [editError, setEditError] = useState<string | null>(null)
  const [editForm, setEditForm] = useState<UpdateUserRequestDTO>({
    id: '',
    firstName: '',
    lastName: '',
    userName: '',
    email: '',
    avatarPath: '',
    covenant: '',
  })

  useEffect(() => {
    if (!isLoading && !user) {
      router.push('/')
    }
  }, [isLoading, router, user])

  useEffect(() => {
    if (user) {
      setEditForm({
        id: user.id,
        firstName: user.firstName,
        lastName: user.lastName,
        userName: user.userName,
        email: user.email,
        avatarPath: user.avatarPath,
        covenant: user.covenant,
      })
    }
  }, [user])

  const handleLogout = useCallback(() => {
    logout()
    router.push('/')
  }, [logout, router])

  const handleAvatarUpload = useCallback(async (event: ChangeEvent<HTMLInputElement>) => {
    const file = event.target.files?.[0]
    if (!file || !user) return

    setIsUploadingAvatar(true)
    setEditError(null)

    const formData = new FormData()
    formData.append('file', file)
    formData.append('Name', `${user.userName} Avatar`)
    formData.append('Route', `users/${user.id}`)

    try {
      const tokens = tokenStorage.getTokens()
      const response = await fetch('/api/content/ContentItem/upload', {
        method: 'POST',
        headers: {
          Authorization: `Bearer ${tokens?.accessToken}`,
        },
        body: formData,
      })

      if (response.ok) {
        setAvatarCacheBuster(Date.now())
        await updateProfile({ ...editForm, avatarPath: `users/${user.id}` })
      } else {
        setEditError('Не вдалося завантажити зображення')
      }
    } catch {
      setEditError('Помилка мережі при завантаженні зображення')
    } finally {
      setIsUploadingAvatar(false)
    }
  }, [editForm, updateProfile, user])

  const handleEditProfile = useCallback(() => {
    setIsEditing(true)
    setEditError(null)
  }, [])

  const handleCancelEdit = useCallback(() => {
    setIsEditing(false)
    setEditError(null)

    if (user) {
      setEditForm({
        id: user.id,
        firstName: user.firstName,
        lastName: user.lastName,
        userName: user.userName,
        email: user.email,
        avatarPath: user.avatarPath,
        covenant: user.covenant,
      })
    }
  }, [user])

  const handleSaveProfile = useCallback(async () => {
    if (!user) {
      setEditError('User is not loaded')
      return
    }

    setIsSaving(true)
    setEditError(null)

    const result = await updateProfile({
      id: user?.id,
      firstName: editForm.firstName,
      lastName: editForm.lastName,
      userName: editForm.userName,
      email: editForm.email,
      covenant: editForm.covenant,
      avatarPath: user?.avatarPath ?? '',
    })

    setIsSaving(false)

    if (result.success) {
      setIsEditing(false)
    } else {
      setEditError(result.error || 'Failed to update profile')
    }
  }, [editForm, updateProfile, user])

  const updateEditFormField = useCallback(<K extends keyof UpdateUserRequestDTO>(field: K, value: UpdateUserRequestDTO[K]) => {
    setEditForm((prev) => ({ ...prev, [field]: value }))
  }, [])

  const toggleAdminPanel = useCallback(() => {
    setShowAdminPanel((prev) => !prev)
  }, [])

  const closeAdminPanel = useCallback(() => {
    setShowAdminPanel(false)
  }, [])

  return {
    adminTab,
    avatarCacheBuster,
    closeAdminPanel,
    editError,
    editForm,
    fileInputRef,
    handleAvatarUpload,
    handleCancelEdit,
    handleEditProfile,
    handleLogout,
    handleSaveProfile,
    isAdmin,
    isEditing,
    isLoading,
    isSaving,
    isUploadingAvatar,
    setAdminTab,
    showAdminPanel,
    toggleAdminPanel,
    updateEditFormField,
    user,
  }
}
