import tensorflow as tf
from tensorflow.keras.models import load_model
from tensorflow.keras.applications.resnet import preprocess_input
import numpy as np
from PIL import Image

# טעינת המודל
model = load_model("resnet_gender_classifier.h5")

# פונקציה להכנת תמונה למודל
def prepare_image(image_path, target_size=(224, 224)):
    """
    טוענת תמונה, משנה את הגודל שלה ומכינה אותה למודל.
    """
    img = Image.open(image_path).convert("RGB")  # המרה ל-RGB
    img = img.resize(target_size)  # שינוי גודל
    img_array = np.array(img)  # המרה למערך
    img_array = preprocess_input(img_array)  # עיבוד מקדים
    img_array = np.expand_dims(img_array, axis=0)  # הוספת ממד batch
    return img_array

# נתיב לתמונה לבדיקה
image_path = r"D:\ai\1tensorflow\traindata\women\00000130.jpg"

# הכנת התמונה
input_image = prepare_image(image_path)

# ביצוע תחזית
predictions = model.predict(input_image)

# מיפוי תוויות
class_labels = ["male", "female"]  # תוויות לפי הסדר שאומן

# קבלת הסתברות לכל קטגוריה
male_probability = predictions[0][0] * 100
female_probability = predictions[0][1] * 100

# זיהוי הקטגוריה עם ההסתברות הגבוהה ביותר
predicted_class = np.argmax(predictions, axis=1)[0]

# תוצאה
print(f"Predicted class: {class_labels[predicted_class]}")
print(f"Probability of Male: {male_probability:.2f}%")
print(f"Probability of Female: {female_probability:.2f}%")
