using System.Collections.Generic;
using System.ComponentModel;
using System.IO;

namespace WTManager
{
    public class ServiceCommand
    {
        public string Name { get; set; }
        public string Arguments { get; set; }
        public string Command { get; set; }
    }

    public class ServiceConfigData
    {
        /// <summary>
        /// �������� �������
        /// </summary>
        public string ServiceName { get; set; }

        /// <summary>
        /// ������������ ���
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// ������������ ����
        /// </summary>
        public int UsedPort { get; set; }

        /// <summary>
        /// ����� � ����� ����
        /// </summary>
        public List<string> Logs { get; set; }

        /// <summary>
        /// ������� ���� � ����������
        /// </summary>
        public string BasePath { get; set; }

        /// <summary>
        /// ���������������� �����
        /// </summary>
        public IEnumerable<string> ConfFiles { get; set; }

        public IEnumerable<ServiceCommand> Commands { get; set; }

        public bool OpenInBrowser { get; set; }

        public string DataDirectory { get; set; }

        public string Group { get; set; }
    }
}