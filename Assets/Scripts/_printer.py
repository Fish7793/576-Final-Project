import glob
from PIL import Image

def find(ext):
    for file in glob.glob('**/*.{}'.format(ext), recursive=True):
        print(("Scripts\\" + file).replace("\\", "/"))
        print()
find("cs")