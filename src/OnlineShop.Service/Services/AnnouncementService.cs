using Microsoft.EntityFrameworkCore;
using OnlineShop.DataAccess.Interfaces.Common;
using OnlineShop.Domain.Entities;
using OnlineShop.Service.Common.Utils;
using OnlineShop.Service.Dtos.Announcement;
using OnlineShop.Service.Interfaces;
using OnlineShop.Service.Interfaces.Common;
using OnlineShop.Service.ViewModels;

namespace OnlineShop.Service.Services
{
    public class AnnouncementService : IAnnouncementService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileService _fileService;
        private readonly IPaginatorService _paginatorService;


        public AnnouncementService(IUnitOfWork unitOfWork, IFileService fileService, IPaginatorService paginatorService)
        {
            this._unitOfWork = unitOfWork;
            this._fileService = fileService;
            this._paginatorService = paginatorService;
        }

        public async Task<bool> CreateAsync(CreateAnnouncementDto announcements)
        {
            try
            {
                var announcement = (Announcement)announcements;

                announcement.CreateAt = DateTime.UtcNow.ToString();
                announcement.UserId = GlobalVariables.Id;
                announcement.LiceCount = 0;

                if (announcements.Image is not null)
                {
                    announcement.ImagePath = await _fileService.SaveImageAsync(announcements.Image);
                }

                _unitOfWork.Announcements.Create(announcement);
                var res = await _unitOfWork.SaveChangesAsync();
                return res > 0;
            }
            catch
            {
                return false;
            }
        }



        public async Task<bool> DeleteAsync(long id)
        {
            try
            {
                _unitOfWork.Announcements.Delete(id);
                _unitOfWork.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }

        }


        public async Task<IList<AnnouncementViewModel>> GetAllAsync(PaginationParams @paginationParams)
        {

            IList<AnnouncementViewModel> list = new List<AnnouncementViewModel>();
            var query = await _unitOfWork.Announcements.GetAll().Where(x => x.LiceCount == 1).OrderBy(x => x.Id).AsNoTracking().ToListAsync();
            foreach (var item in query)
            {
                AnnouncementViewModel announcementViewModel = new AnnouncementViewModel();
                announcementViewModel.Id = item.Id;
                announcementViewModel.Title = item.Title;
                announcementViewModel.Price = item.Price;
                announcementViewModel.PhoneNumber = item.PhoneNumber;
                announcementViewModel.Description = item.Description;
                announcementViewModel.ImagePath = item.ImagePath;
                announcementViewModel.CreateAt = item.CreateAt;
                list.Add(announcementViewModel);
            }
            return list;
        }

        public async Task<IList<AnnouncementViewModel>> GetAllAsyncAdmin()
        {
            IList<AnnouncementViewModel> list = new List<AnnouncementViewModel>();
            var query = await _unitOfWork.Announcements.GetAll().Where(x => x.LiceCount == 0).OrderBy(x => x.Id).AsNoTracking().ToListAsync();
            foreach (var item in query)
            {
                AnnouncementViewModel announcementViewModel = new AnnouncementViewModel();
                announcementViewModel.Id = item.Id;
                announcementViewModel.Title = item.Title;
                announcementViewModel.Price = item.Price;
                announcementViewModel.PhoneNumber = item.PhoneNumber;
                announcementViewModel.Description = item.Description;
                announcementViewModel.ImagePath = item.ImagePath;
                announcementViewModel.CreateAt = item.CreateAt;
                list.Add(announcementViewModel);
            }
            return list;
        }

        public async Task GetAllAsyncAdminAdd(long id)
        {
            var query = await _unitOfWork.Announcements.FirstByIdAsync(id);
            _unitOfWork.Announcements.TrackingDeteched(query);
            query.LiceCount = 1;
            _unitOfWork.Announcements.Update(id, query);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task GetAllAsyncAdminRemove(long id)
        {
            var query = await _unitOfWork.Announcements.FirstByIdAsync(id);
            _unitOfWork.Announcements.TrackingDeteched(query);
            query.LiceCount = 2;
            _unitOfWork.Announcements.Update(id, query);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<IList<AnnouncementViewModel>> GetAllAsyncUser(PaginationParams paginationParams)
        {
            IList<AnnouncementViewModel> list = new List<AnnouncementViewModel>();
            var query = await _unitOfWork.Announcements.GetAll().Where(x => x.UserId == GlobalVariables.Id).OrderBy(x => x.Id).AsNoTracking().ToListAsync();
            foreach (var item in query)
            {
                AnnouncementViewModel announcementViewModel = new AnnouncementViewModel();
                announcementViewModel.Id = item.Id;
                announcementViewModel.Title = item.Title;
                announcementViewModel.Price = item.Price;
                announcementViewModel.PhoneNumber = item.PhoneNumber;
                announcementViewModel.Description = item.Description;
                announcementViewModel.ImagePath = item.ImagePath;
                announcementViewModel.CreateAt = item.CreateAt;
                list.Add(announcementViewModel);
            }
            return list;
        }

        public async Task<IList<AnnouncementViewModel>> GetAllCategoryAsync(int id)
        {
            IList<AnnouncementViewModel> list = new List<AnnouncementViewModel>();
            var query = await _unitOfWork.Announcements.GetAll().Where(x => x.CategoryId == id && x.LiceCount == 1).OrderBy(x => x.Id).AsNoTracking().ToListAsync();
            foreach (var item in query)
            {
                AnnouncementViewModel announcementViewModel = new AnnouncementViewModel();
                announcementViewModel.Id = item.Id;
                announcementViewModel.Title = item.Title;
                announcementViewModel.Price = item.Price;
                announcementViewModel.PhoneNumber = item.PhoneNumber;
                announcementViewModel.Description = item.Description;
                announcementViewModel.ImagePath = item.ImagePath;
                list.Add(announcementViewModel);
            }
            return list;
        }

        public async Task<IList<AnnouncementViewModel>> GetByIdAsync(long id)
        {
            try
            {
                IList<AnnouncementViewModel> announcementViewModels = new List<AnnouncementViewModel>();

                var resault = await _unitOfWork.Announcements.FirstByIdAsync(id);
                AnnouncementViewModel announcementViewModel = new AnnouncementViewModel();
                announcementViewModel.Id = resault.Id;
                announcementViewModel.Title = resault.Title;
                announcementViewModel.Price = resault.Price;
                announcementViewModel.PhoneNumber = resault.PhoneNumber;
                announcementViewModel.ImagePath = resault.ImagePath;
                announcementViewModel.Description = resault.Description;
                announcementViewModels.Add(announcementViewModel);
                return announcementViewModels;
            }
            catch
            {
                return null;
            }

        }

        public async Task<bool> UpdateAsync(long id, Announcement announcement)
        {
            try
            {
                announcement.CreateAt = DateTime.UtcNow.ToString();
                announcement.UserId = GlobalVariables.Id;
                announcement.LiceCount = 0;


                _unitOfWork.Announcements.Update(id, announcement);
                var res = await _unitOfWork.SaveChangesAsync();
                return res > 0;

            }
            catch
            {
                return false;
            }
        }


    }
}
