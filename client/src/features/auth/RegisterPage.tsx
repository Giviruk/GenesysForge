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
            Back to dashboard
          </NavLink>
          <h1 id="register-heading">Create account</h1>
          <p>Start a workspace for your characters, rulesets, and future sheets.</p>
        </header>

        <form className="auth-form" onSubmit={handleSubmit(handleRegister)}>
          <label>
            Display name
            <input
              {...register('displayName', {
                required: 'Display name is required',
                maxLength: { value: 100, message: 'Use 100 characters or fewer' },
              })}
              autoComplete="name"
            />
          </label>
          {errors.displayName ? <p className="auth-error">{errors.displayName.message}</p> : null}

          <label>
            Email
            <input
              {...register('email', {
                required: 'Email is required',
                pattern: {
                  value: /^[^\s@]+@[^\s@]+\.[^\s@]+$/,
                  message: 'Use a valid email address',
                },
              })}
              autoComplete="email"
              type="email"
            />
          </label>
          {errors.email ? <p className="auth-error">{errors.email.message}</p> : null}

          <label>
            Password
            <input
              {...register('password', {
                required: 'Password is required',
                minLength: { value: 8, message: 'Use at least 8 characters' },
                maxLength: { value: 128, message: 'Use 128 characters or fewer' },
              })}
              autoComplete="new-password"
              type="password"
            />
          </label>
          {errors.password ? <p className="auth-error">{errors.password.message}</p> : null}

          {mutation.isError ? <p className="auth-error">{getErrorMessage(mutation.error)}</p> : null}

          <button className="auth-submit" disabled={mutation.isPending} type="submit">
            {mutation.isPending ? 'Creating account...' : 'Create account'}
          </button>
        </form>

        <NavLink to="/login" className="auth-secondary">
          I already have an account
        </NavLink>
      </section>
    </main>
  )
}

function getErrorMessage(error: Error) {
  if (error instanceof ApiError) {
    return error.message
  }

  return 'Could not create account.'
}
