import type { ProjectStatus } from './projectStatus/ProjectStatus'

const projectStatus: ProjectStatus[] = [
  {
    label: 'Backend',
    value: '.NET 10',
    detail: 'Clean Architecture skeleton is ready',
  },
  {
    label: 'Frontend',
    value: 'Vite',
    detail: 'React app shell is online',
  },
  {
    label: 'Workflow',
    value: 'Draft',
    detail: 'Character creation starts here',
  },
]

export async function getProjectStatus() {
  await new Promise((resolve) => window.setTimeout(resolve, 120))

  return projectStatus
}
