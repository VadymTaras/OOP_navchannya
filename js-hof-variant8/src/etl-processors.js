import { curry } from "./fp-core.js";

/**
 * Крок Extraction/Validation: валідація та фільтрація некоректних вхідних даних.
 */
export const extractAndValidate = (rawData) => {
    if (!Array.isArray(rawData)) return { items: [], totalCount: 0 };

    // Фільтруємо об'єкти, де відсутні обов'язкові поля
    const validItems = rawData.filter(item => item && item.id && item.price !== undefined);
    return { items: validItems, totalCount: validItems.length };
};

/**
 * Крок Transformation: кастомний каррований мапінг цін із урахуванням знижки та ПДВ.
 */
export const transformPrices = curry((discountRate, taxRate, data) => {
    if (data.failed) return data;

    const updatedItems = data.items.map(item => {
        const priceAfterDiscount = item.price * (1 - discountRate);
        const finalPrice = priceAfterDiscount * (1 + taxRate);
        return {
            ...item,
            discountedPrice: Number(priceAfterDiscount.toFixed(2)),
            finalPrice: Number(finalPrice.toFixed(2))
        };
    });

    return { ...data, items: updatedItems };
});

/**
 * Умовне розгалуження: Фільтрація преміум-товарів за категоріями або ціною.
 */
export const filterPremiumItems = (data) => {
    if (data.failed) return data;
    const premium = data.items.filter(item => item.finalPrice > 500);
    return { ...data, items: premium, mode: "Premium-Only" };
};

export const keepAllItems = (data) => {
    if (data.failed) return data;
    return { ...data, mode: "Standard-All" };
};

/**
 * Крок Loading/Aggregation: агрегація фінальної аналітики через reduce.
 */
export const aggregateAnalytics = (data) => {
    if (data.failed) return data;

    const analytics = data.items.reduce((acc, item) => {
        acc.totalRevenue = acc.totalRevenue + item.finalPrice;
        acc.categories[item.category] = (acc.categories[item.category] || 0) + 1;
        return acc;
    }, { totalRevenue: 0, categories: {} });

    analytics.totalRevenue = Number(analytics.totalRevenue.toFixed(2));

    return {
        ...data,
        analytics
    };
};