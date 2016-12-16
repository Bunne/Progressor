# Progressor

Progressor is a small program for creating and maintaining task progress lists via the command line, as well as generating Markdown documents to present those lists. An example can be found [here](https://bunne.github.io/progress/).

Tasks can be created and updated manually via the command line. Git repositories can also be specified via the generated configuration file. When a Markdown page is generated, updates for git repositories are drawn from the master branch. Information is stored locally in a .bin file for manually-maintained tasks.

### Commands

* `create -d <taskName>` : Create a Task.
* `delete -t <taskId>` : Delete a Task.
* `update -t <taskId> -u <update>` : Apply an Update to a Task.
* `ltasks` : List all Tasks and their associated IDs.
* `lupdates -t <taskId>` : List Updates for a specified task.
* `markdown` : Generate a .md file from all Tasks.

### Side-Note

This started off a quick project for picking up C# from Java in a weekend's time. I liked where it was going, and I may extend it or play with it in the future. I'd like to focus on expanding the git-to-markdown aspect and make it its own project at some point. 
