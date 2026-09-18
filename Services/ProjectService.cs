using DemoApi.Models;
using DemoApi.Repositories;

namespace DemoApi.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectService(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<List<Project>> GetAllProjectsAsync()
        {
            return await _projectRepository.GetAllAsync();
        }

        public async Task<Project?> GetProjectByIdAsync(int id)
        {
            return await _projectRepository.GetByIdAsync(id);
        }

        public async Task<Project> CreateProjectAsync(Project project)
        {
            if (string.IsNullOrWhiteSpace(project.Name))
            {
                throw new ArgumentException("Project name cannot be empty.");
            }

            return await _projectRepository.AddAsync(project);
        }

        public async Task<Project?> UpdateProjectAsync(int id, Project project)
        {
            if (string.IsNullOrWhiteSpace(project.Name))
            {
                throw new ArgumentException("Project name cannot be empty.");
            }

            return await _projectRepository.UpdateAsync(id, project);
        }

        public async Task<bool> DeleteProjectAsync(int id)
        {
            return await _projectRepository.DeleteAsync(id);
        }
    }
}
