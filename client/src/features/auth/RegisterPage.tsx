import { useMutation } from '@tanstack/react-query'
import { useForm } from 'react-hook-form'
import { NavLink, useNavigate } from 'react-router-dom'
import { ApiError, login, register as registerAccount } from '../../api/authApi'
import type { RegisterRequest } from '../../api/auth/RegisterRequest'
import { useAuthStore } from '../../stores/useAuthStore'
import './AuthPage.css'

export function RegisterPage() {
  const navigate = useNavigate()
  const setSession = useAuthStore((state) => state.setSession)
  const mutation = useMutation({
    mutationFn: async (values: RegisterRequest) => {
      await registerAccount(values)
      return login({ email: values.email, password: values.password })
    },
    onSuccess: (session) => {
      setSession(session)
      navigate('/')
    },
  })
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<RegisterRequest>({
    defaultValues: {
      email: '',
      password: '',
      displayName: '',
    },
  })

  function handleRegister(values: RegisterRequest) {
    mutation.mutate(values)
  }

  return (
    <main className="auth-page">
      <section className="auth-panel" aria-labelledby="register-heading">
        <header>
          <NavLink to="/" className="auth-secondary">
            Вернуться на панель
          </NavLink>
          <h1 id="register-heading">Создание аккаунта</h1>
          <p>Откройте рабочее пространство для персонажей, наборов правил и будущих листов.</p>
        </header>

        <form className="auth-form" onSubmit={handleSubmit(handleRegister)}>
          <label>
            Отображаемое имя
            <input
              {...register('displayName', {
                required: 'Укажите отображаемое имя',
                maxLength: { value: 100, message: 'Введите не больше 100 символов' },
              })}
              autoComplete="name"
            />
          </label>
          {errors.displayName ? <p className="auth-error">{errors.displayName.message}</p> : null}

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
                minLength: { value: 8, message: 'Введите минимум 8 символов' },
                maxLength: { value: 128, message: 'Введите не больше 128 символов' },
              })}
              autoComplete="new-password"
              type="password"
            />
          </label>
          {errors.password ? <p className="auth-error">{errors.password.message}</p> : null}

          {mutation.isError ? <p className="auth-error">{getErrorMessage(mutation.error)}</p> : null}

          <button className="auth-submit" disabled={mutation.isPending} type="submit">
            {mutation.isPending ? 'Создаем аккаунт...' : 'Создать аккаунт'}
          </button>
        </form>

        <NavLink to="/login" className="auth-secondary">
          У меня уже есть аккаунт
        </NavLink>
      </section>
    </main>
  )
}

function getErrorMessage(error: Error) {
  if (error instanceof ApiError) {
    return error.message
  }

  return 'Не удалось создать аккаунт.'
}
