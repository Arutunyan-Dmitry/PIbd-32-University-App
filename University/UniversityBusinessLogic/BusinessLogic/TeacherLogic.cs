using System.Text.RegularExpressions;
using UniversityContracts.BindingModels;
using UniversityContracts.BusinessLogicContracts;
using UniversityContracts.StorageContracts;
using UniversityContracts.ViewModels;

namespace UniversityBusinessLogic.BusinessLogic
{
    public class TeacherLogic : ITeacherLogic
    {
        private readonly ITeacherStorage _teacherStorage;
        private readonly IDisciplineStorage _disciplineStorage;
        private readonly int _passwordMaxLength = 50;
        private readonly int _passwordMinLength = 10;
        public TeacherLogic(ITeacherStorage teacherStorage, IDisciplineStorage disciplineStorage)
        {
            _teacherStorage = teacherStorage;
            _disciplineStorage = disciplineStorage;
        }
        public List<TeacherViewModel> Read(TeacherBindingModel model)
        {
            if (model == null)
            {
                return _teacherStorage.GetFullList();
            }
            if (model.Id.HasValue || (model.Login != null && model.Password != null))
            {
                return new List<TeacherViewModel> { _teacherStorage.GetElement(model) };
            }
            return _teacherStorage.GetFilteredList(model);
        }
        public List<TeacherViewModel> GetTeacherByDiscipline(int id)
        {
            return _teacherStorage.GetTeacherByDiscipline(id);
        }
        public void CreateOrUpdate(TeacherBindingModel model)
        {
            var element = _teacherStorage.GetElement(new TeacherBindingModel
            {
                DepartmentId = model.DepartmentId,
                Flm = model.Flm,
                Login = model.Login,
                Password = model.Password
            });
            if (element != null && element.Id != model.Id)
            {
                throw new Exception("Уже есть такой элемент");
            }
            if (model.Password.Length > _passwordMaxLength || model.Password.Length < _passwordMinLength ||
                !Regex.IsMatch(model.Password, @"^((\w+\d+\W+)|(\w+\W+\d+)|(\d+\w+\W+)|(\d+\W+\w+)|(\W+\w+\d+)|(\W+\d+\w+))[\w\d\W]*$"))
            {
                throw new Exception($"Пароль должен быть длиной от {_passwordMinLength} до " +
                    $"{_passwordMaxLength} и состоять из цифр, букв и небуквенных символов");
            }
            if (model.Id.HasValue)
            {
                _teacherStorage.Update(model);
            }
            else
            {
                _teacherStorage.Insert(model);
            }
        }
        public void Delete(TeacherBindingModel model)
        {
            var element = _teacherStorage.GetElement(new TeacherBindingModel
            {
                Id = model.Id
            });
            if (element == null)
            {
                throw new Exception("Элемент не найден");
            }
            _teacherStorage.Delete(model);
        }
        public void AddDiscipline(TeacherDisciplineBindingModel model)
        {
            var teacher = _teacherStorage.GetElement(new TeacherBindingModel
            {
                Id = model.TeacherId
            });

            var discipline = _disciplineStorage.GetElement(new DisciplineBindingModel
            {
                Id = model.DisciplineId
            });

            if (teacher == null || discipline == null)
            {
                throw new Exception("Элемент не найден");
            }

            List<int> teacherDisciplines = new List<int>();
            foreach (int key in teacher.TeacherDisciplines.Keys)
                teacherDisciplines.Add(key);

            if (!teacherDisciplines.Contains(model.DisciplineId))
            {
                teacherDisciplines.Add(model.DisciplineId);
            }

            _teacherStorage.Update(new TeacherBindingModel
            {
                Id = teacher.Id,
                DepartmentId = teacher.DepartmentId,
                Flm = teacher.Flm,
                Login = teacher.Login,
                Password = teacher.Password,
                TeacherDisciplines = teacherDisciplines
            });
        }
        public void RemoveDiscipline(TeacherDisciplineBindingModel model)
        {
            var teacher = _teacherStorage.GetElement(new TeacherBindingModel
            {
                Id = model.TeacherId
            });

            var discipline = _disciplineStorage.GetElement(new DisciplineBindingModel
            {
                Id = model.DisciplineId
            });

            if (teacher == null || discipline == null)
            {
                throw new Exception("Элемент не найден");
            }

            List<int> teacherDisciplines = new List<int>();
            foreach (int key in teacher.TeacherDisciplines.Keys)
                teacherDisciplines.Add(key);

            if (teacherDisciplines.Contains(model.DisciplineId))
            {
                teacherDisciplines.Remove(model.DisciplineId);
            }
            else
            {
                throw new Exception("Запись не найдена");
            }

            _teacherStorage.Update(new TeacherBindingModel
            {
                Id = teacher.Id,
                DepartmentId = teacher.DepartmentId,
                Flm = teacher.Flm,
                Login = teacher.Login,
                Password = teacher.Password,
                TeacherDisciplines = teacherDisciplines
            });
        }
    }
}
