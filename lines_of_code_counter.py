import subprocess

SCRIPTS_DIRECTORY = "Assets/Scripts/"
SCRIPT_FILE_EXTENSION = ".cs"

try:
    output = subprocess.check_output([
        'git',
        'log',
        '--pretty=format:"%aN"',
        '--numstat',
        '--no-merges',
        '--',
        SCRIPTS_DIRECTORY + '*' + SCRIPT_FILE_EXTENSION
    ])

    branch_name = subprocess.check_output([
        'git',
        'branch',
        '--show-current'
    ])
except FileNotFoundError:
    print("You need to install git to use this script.")
    exit(1)

output = output.decode("utf-8")
branch_name = branch_name.decode("utf-8").strip()

result_list = output.split("\n\n")
contributor_list = []
contributions_count = {}


for result in result_list:
    result_lines = result.split("\n")
    contributor = result_lines[0]

    if contributor not in contributor_list:
        contributor_list.append(contributor)
        contributions_count[contributor] = [0, 0]

    # Remove contributor name to get only the lines and files changed
    lines_added_list = result_lines[1:]

    for lines_added in lines_added_list:
        lines_added_split = lines_added.split()

        if len(lines_added_split) == 3:
            lines_added_count = int(lines_added_split[0])
            lines_removed_count = int(lines_added_split[1])

            contributions_count[contributor][0] += lines_added_count
            contributions_count[contributor][1] += lines_removed_count


print("\nNote: You are currently on branch \"" + branch_name + "\".")
print("Only the changes in this branch were counted.")
print("Only changes on files in the \"" + SCRIPTS_DIRECTORY + "\" directory were counted.")
print("Only changes on files with the \"" + SCRIPT_FILE_EXTENSION + "\" extension were counted.")
print("Merge commits were not counted.\n")

# Print out each contributor's name followed by the lines of C# code
for key, value in contributions_count.items():
    print('-'*50)
    print(key)
    print(f'+{value[0]}  -{value[1]}')

print('-'*50)
