import json
import random
import sys

def generate_data(world_size_x, world_size_y, agents_count, ilness_probability):
    data = {}
    data['agents'] = []
    existing_agents_cords=set()
    illness=False
    gender='M'
    while len(existing_agents_cords) < agents_count:
        x = random.randint(0, world_size_x-1)
        y = random.randint(0, world_size_y-1)
        cords = (x, y)
        if cords not in existing_agents_cords:
            existing_agents_cords.add(cords)
            rand_illness = random.random()
            if rand_illness < ilness_probability:
                illness=True
            else:
                illness=False
            rand_gender = random.random()
            if rand_gender < 0.5:
                gender='F'
            else:
                gender='M'
            
            data['agents'].append({
                'x': str(x),
                'y': str(y),
                'illness': str(illness),
                'gender': gender
                })
    data['illness']=[]
    fatality=float(random.randint(1, 10)/100)
    infectiousness=float(random.randint(25, 80)/100)
    data['illness'].append({
        'fatality':str(fatality),
        'infectiousness':str(infectiousness)
        })
    return data

if __name__ == "__main__":
    if len(sys.argv)==6:
        filename=sys.argv[1]
        with open("..\..\..\Assets\Data\\"+str(filename)+".json", 'w') as data_file:
            world_size_x = sys.argv[2]
            world_size_y = sys.argv[3]
            agents_count = sys.argv[4]
            ilness_probability = sys.argv[5]
            data = generate_data(int(world_size_x), int(world_size_y), int(agents_count), float(ilness_probability))
            json.dump(data, data_file)
            data_file.close()
    else:
        print("Inncorect arguments count")