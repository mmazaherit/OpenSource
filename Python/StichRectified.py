import numpy as np
import cv2
from PIL import Image
import os
folder = 'D:/path/'
for i in range(0,1):
    epoch=str(i)
    img1=cv2.imread(folder+'1_'+epoch+'.jpeg')
    img2=cv2.imread(folder+'0_'+epoch+'.jpeg')
    height, width , channels = img1.shape
    print height, width
    img1=img1[0:height, 0:2500]    
    img2=img2[0:height, 1500:4000]
    h, w1 , c1 = img1.shape
    h, w2 , c2 = img2.shape
    blank_image = np.zeros((height,w1+w2,3), np.uint8)
    blank_image[:,0:w1,:] = img1      # (B, G, R)
    blank_image[:,w1:(w1+w2),:] = img2
    cv2.imwrite(folder+'stiched_'+epoch+'.jpeg',blank_image)



