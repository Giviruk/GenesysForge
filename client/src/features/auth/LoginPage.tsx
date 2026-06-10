import { useMutation } from '@tanstack/react-query'
import { useForm } from 'react-hook-form'
import { NavLink, useLocation, useNavigate } from 'react-router-dom'
import { ApiError, login } from '../../api/authApi'
import type { LoginRequest } from '../../api/auth/LoginRequest'
import { useAuthStore } from '../../stores/useAuthStore'
import './AuthPage.css'

type LoginLocationState = {
  message?: string
}

export function LoginPage() {
  const navigate = useNavigate()
  const location = useLocation()
  const setSession = useAuthStore((state) => state.setSession)
  const locationState = location.state as LoginLocationState | null
  const mutation = useMutation({
    mutationFn: login,
    onSuccess: (session) => {
      setSession(session)
      navigate('/')
    },
  })
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<LoginRequest>({
    defaultValues: {
      email: '',
      password: '',
    },
  })

  function handleLogin(values: LoginRequest) {
    mutation.mutate(values)
  }

  return (
    <main className="auth-page">
      <section className="auth-panel" aria-labelledby="login-heading">
        <header>
          <NavLink to="/" className="auth-secondary">
            Вернуться на панель
          </NavLink>
          <h1 id="login-heading">Вход</h1>
          <p>Продолжайте создавать персонажей в сохраненном рабочем пространстве.</p>
        </header>

        {locationState?.message ? <p className="auth-message">{locationState.message}</p> : null}

        <form className="auth-form" onSubmit={handleSubmit(handleLogin)}>
          <label>
            Email
            <input
              {...register('email', {
                required: 'Укажите email',
                pattern: {
                  value: /^[^\s@]+@[^\s@]+\.[^\s@]+$/,
                  message: 'Введите корректный email',
                },
              })}
              autoComplete="email"
              type="email"
            />
          </label>
          {errors.email ? <p className="auth-error">{errors.email.message}</p> : null}

          <label>
            Пароль
            <input
              {...register('password', {
                required: 'Укажите пароль',
              })}
              autoComplete="current-password"
              type="password"
            />
          </label>
          {errors.password ? <p className="auth-error">{errors.password.message}</p> : null}

          {mutation.isError ? <p className="auth-error">{getErrorMessage(mutation.error)}</p> : null}

          <button className="auth-submit" disabled={mutation.isPending} type="submit">
            {mutation.isPending ? 'Входим...' : 'Войти'}
          </button>
        </form>

        <NavLink to="/register" className="auth-secondary">
          Создать аккаунт
        </NavLink>
      </section>
    </main>
  )
}

function getErrorMessage(error: Error) {
  if (error instanceof ApiError) {
    return error.message
  }

  return 'Не удалось войти.'
}
