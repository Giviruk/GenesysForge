import type { ProjectStatus } from './projectStatus/ProjectStatus'

const projectStatus: ProjectStatus[] = [
  {
    label: 'Бэкенд',
    value: '.NET 10',
    detail: 'Основа Clean Architecture готова',
  },
  {
    label: 'Фронтенд',
    value: 'Vite',
    detail: 'Каркас React-приложения запущен',
  },
  {
    label: 'Процесс',
    value: 'Черновик',
    detail: 'Создание персонажа начинается здесь',
  },
]

export async function getProjectStatus(): Promise<ProjectStatus[]> {
  await new Promise((resolve) => window.setTimeout(resolve, 120))

  return projectStatus
}
