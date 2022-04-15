namespace Restaurant.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Restaurant.Data.Common.Repositories;
    using Restaurant.Data.Models;
    using Restaurant.Services.Mapping;
    using Restaurant.Services.Models;

    public class UserMessageService : IUserMessageService
    {
        private readonly IDeletableEntityRepository<UserMessage> userMessageRepository;
        private readonly IDeletableEntityRepository<UserMessageCategory> messageCategoryRepository;

        public UserMessageService(IDeletableEntityRepository<UserMessage> userMessageRepository, IDeletableEntityRepository<UserMessageCategory> messageCategoryRepository)
        {
            this.userMessageRepository = userMessageRepository;
            this.messageCategoryRepository = messageCategoryRepository;
        }

        public async Task<string> CreateAsync(AddUserMessageModel model)
        {
            var userMessage = AutoMapperConfig.MapperInstance.Map<UserMessage>(model);
            await this.userMessageRepository.AddAsync(userMessage);
            await this.userMessageRepository.SaveChangesAsync();
            return userMessage.Id;
        }

        public IEnumerable<T> GetCategories<T>()
        {
            return this.messageCategoryRepository.AllAsNoTracking().To<T>().ToList();
        }
    }
}
