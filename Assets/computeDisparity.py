
import random


class ComputeDisparity():
    
    #constructor
    def __init__(self, name):
        self.name = name

    #functions
    def greet(self):
        return "This is Camera: " + self.name

    def random_number(self, start, end):
    	return random.randint(start, end)