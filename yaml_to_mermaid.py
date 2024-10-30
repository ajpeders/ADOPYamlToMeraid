import yaml
import re
import os

class YamlMermaidParser:
    def __init__(self, yaml_file, chart_type="flowchart TD", parameters=None, variables=None, name_prefix=""):
        self.chart_type = chart_type
        self.yaml_file = yaml_file
        self.pipeline = self.load_yaml()
        self.steps_list = []
        self.step_counter = 0
        self.parameters = parameters or {}
        self.variables = variables or {}
        self.name_prefix = name_prefix

    def load_yaml(self):
        with open(self.yaml_file, 'r') as file:
            return yaml.safe_load(file)

    def set_parameters(self, parameters):
        self.parameters = {**self.pipeline.get('parameters', {}), **parameters}

    def set_variables(self, variables):
        self.variables = {**self.pipeline.get('variables', {}), **variables}

    def replace_parameters(self, steps, parameters):
        steps_str = yaml.dump(steps)
        for key, value in parameters.items():
            steps_str = steps_str.replace(f"${{{{ parameters.{key} }}}}", str(value))
        return yaml.safe_load(steps_str)

    def extract_steps(self, steps):
        steps = self.replace_parameters(steps, self.parameters)
        for step in steps:
            if 'template' in step:
                self.process_template(step)
            elif 'task' in step or 'script' in step:
                self.process_step(step)

    def process_step(self, step):
        step_name = step.get('displayName') or step.get('task') or step.get('script') or 'Unnamed Step'
        unique_step_name = self.get_unique_name(step_name)
        sanitized_step_name = re.sub(r'[^\w\s]', '', step_name)

        step_type = "Task" if 'task' in step else "Script"
        mermaid_string = f"    {unique_step_name}[{step_type}: {sanitized_step_name}]"

        self.steps_list.append((unique_step_name, mermaid_string, None))

    def process_template(self, step):
        template_parameters = {**self.parameters, **step.get('parameters', {})}
        template_path = step['template']
        template_name = os.path.basename(template_path)
        unique_template_name = self.get_unique_name(template_name)

        template_parser = YamlMermaidParser(template_path, parameters=template_parameters, name_prefix=unique_template_name + "_")
        template_mermaid_output = template_parser.parse()
        template_mermaid_lines = template_mermaid_output.split('\n')[1:]

        mermaid_string = self.create_template_subgraph(unique_template_name, template_name, template_mermaid_lines)
        self.steps_list.append((unique_template_name, mermaid_string, template_parameters))

    def create_template_subgraph(self, unique_template_name, template_name, template_mermaid_lines):
        mermaid_string = f"    subgraph {unique_template_name} [Template: {template_name}]\n"
        for line in template_mermaid_lines:
            if line.strip() and not line.startswith("subgraph") and not line.startswith("end"):
                mermaid_string += f"{line}\n"
        mermaid_string += "    end\n"
        return mermaid_string

    def extract_jobs(self, jobs):
        jobs = self.replace_parameters(jobs, self.parameters)
        for job in jobs:
            self.process_job(job)

    def process_job(self, job):
        job_name = job.get('job', 'Unnamed Job')
        unique_job_name = self.get_unique_name(job_name)
        sanitized_job_name = re.sub(r'[^\w\s]', '', job_name)
        mermaid_string = f"    {unique_job_name}[Job: {sanitized_job_name}]"
        self.steps_list.append((unique_job_name, mermaid_string))
        self.extract_steps(job.get('steps', []))

    def get_unique_name(self, name):
        unique_name = f"{self.name_prefix}{self.step_counter}_{name}"
        self.step_counter += 1
        return re.sub(r'[^\w-]', '', unique_name).replace(' ', '_')

    def create_group_legend(self, name_prefix, items, legend_type):
        if not items:
            return ""
        legend_name = f"{name_prefix}{legend_type.lower()}_Legend"
        legend_output = f"  subgraph {legend_name} [{legend_type}]\n    direction LR\n"
        for key, value in items.items():
            sanitized_key = f"{name_prefix}{key}".replace(' ', '_')
            legend_output += f"    {sanitized_key}[{key}: {value}]\n"
        legend_output += "  end\n"
        return legend_output

    def parse(self):
        self.set_parameters(self.parameters)
        self.set_variables(self.variables)
        mermaid_output = f"{self.chart_type}\n"

        if 'jobs' in self.pipeline:
            self.extract_jobs(self.pipeline['jobs'])
        elif 'steps' in self.pipeline:
            self.extract_steps(self.pipeline['steps'])

        mermaid_output += self.create_group_legend(self.name_prefix, self.parameters, 'Parameters')
        mermaid_output += self.create_group_legend(self.name_prefix, self.variables, 'Variables')

        mermaid_output = self.generate_mermaid_output(mermaid_output)

        self.mermaid_output = mermaid_output
        return self.mermaid_output

    def generate_mermaid_output(self, mermaid_output):
        previous_step = None
        for step in self.steps_list:
            step_name, mermaid_string, passed_params = step

            mermaid_output += f"{mermaid_string}\n"

            if previous_step and "[" in mermaid_string:
                if passed_params:
                    param_str = ", ".join([f"{key}: {value}" for key, value in passed_params.items()])
                    mermaid_output += f"    {previous_step} --> |{param_str}| {step_name}\n"
                else:
                    mermaid_output += f"    {previous_step} --> {step_name}\n"
            previous_step = step_name
        return mermaid_output

# Example usage
yaml_file = 'azure-pipelines.yml'
parser = YamlMermaidParser(yaml_file)
mermaid_diagram = parser.parse()
print(mermaid_diagram)

# Optionally, save the output to a file
with open('pipeline_diagram.mmd', 'w') as output_file:
    output_file.write(mermaid_diagram)
