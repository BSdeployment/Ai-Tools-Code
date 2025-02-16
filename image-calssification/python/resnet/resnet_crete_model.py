import tensorflow as tf
from tensorflow.keras.preprocessing import image_dataset_from_directory
from tensorflow.keras import layers
from tensorflow.keras.models import Sequential
from tensorflow.keras.applications import ResNet50
from tensorflow.keras.applications.resnet import preprocess_input

# הגדרות נתיב לתקיות
train_dir = r"D:\ai\1tensorflow\traindata"
validation_dir = r"D:\ai\1tensorflow\testdata"

# פרמטרים בסיסיים
batch_size = 32
img_size = (224, 224)  # ResNet מצפה לגודל זה

# יצירת סט הנתונים
train_dataset = image_dataset_from_directory(
    train_dir,
    shuffle=True,
    batch_size=batch_size,
    image_size=img_size,
)

validation_dataset = image_dataset_from_directory(
    validation_dir,
    shuffle=True,
    batch_size=batch_size,
    image_size=img_size,
)

# הוספת עיבוד מקדים
def preprocess(dataset):
    return dataset.map(lambda x, y: (preprocess_input(x), y))

train_dataset = preprocess(train_dataset)
validation_dataset = preprocess(validation_dataset)

# שיפור ביצועים
AUTOTUNE = tf.data.AUTOTUNE
train_dataset = train_dataset.cache().prefetch(buffer_size=AUTOTUNE)
validation_dataset = validation_dataset.cache().prefetch(buffer_size=AUTOTUNE)

# יצירת מודל ResNet50
base_model = ResNet50(include_top=False, weights='imagenet', input_shape=(224, 224, 3))
base_model.trainable = False  # קיפאון משקלות הבסיס

model = Sequential([
    layers.Input(shape=(224, 224, 3)),
    base_model,
    layers.GlobalAveragePooling2D(),
    layers.Dropout(0.2),
    layers.Dense(2, activation='softmax')  # 2 קטגוריות: male / female
])

model.compile(
    optimizer='adam',
    loss='sparse_categorical_crossentropy',
    metrics=['accuracy']
)

# אימון המודל
epochs = 10
history = model.fit(
    train_dataset,
    validation_data=validation_dataset,
    epochs=epochs
)

# שמירת המודל
model.save("resnet_gender_classifier.h5")
