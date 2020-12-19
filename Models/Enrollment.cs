using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContosoUniversity.Models
{

    public enum Grade
    {
        A, B, C, D, F
    }

    public class Enrollment
    {
        // Note: When ICollection<Enrollment> is used, EF Core creates a HashSet<Enrollment> collection by default.
        public int EnrollmentID { get; set; }
        public int CourseID { get; set; }
        public int StudentID { get; set; }
        // Question mark means Nullable.
        [DisplayFormat(NullDisplayText = "No grade")]
        public Grade? Grade { get; set; }

        public Course Course { get; set; }
        public Student Student { get; set; }
    }
}
