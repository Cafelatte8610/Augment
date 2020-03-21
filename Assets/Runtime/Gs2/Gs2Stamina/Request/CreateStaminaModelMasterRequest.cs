/*
 * Copyright 2016 Game Server Services, Inc. or its affiliates. All Rights
 * Reserved.
 *
 * Licensed under the Apache License, Version 2.0 (the "License").
 * You may not use this file except in compliance with the License.
 * A copy of the License is located at
 *
 *  http://www.apache.org/licenses/LICENSE-2.0
 *
 * or in the "license" file accompanying this file. This file is distributed
 * on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either
 * express or implied. See the License for the specific language governing
 * permissions and limitations under the License.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using Gs2.Core.Control;
using Gs2.Core.Model;
using Gs2.Gs2Stamina.Model;
using LitJson;
using UnityEngine.Scripting;

namespace Gs2.Gs2Stamina.Request
{
	[Preserve]
	[System.Serializable]
	public class CreateStaminaModelMasterRequest : Gs2Request<CreateStaminaModelMasterRequest>
	{

        /** ネームスペース名 */
		[UnityEngine.SerializeField]
        public string namespaceName;

        /**
         * ネームスペース名を設定
         *
         * @param namespaceName ネームスペース名
         * @return this
         */
        public CreateStaminaModelMasterRequest WithNamespaceName(string namespaceName) {
            this.namespaceName = namespaceName;
            return this;
        }


        /** スタミナの種類名 */
		[UnityEngine.SerializeField]
        public string name;

        /**
         * スタミナの種類名を設定
         *
         * @param name スタミナの種類名
         * @return this
         */
        public CreateStaminaModelMasterRequest WithName(string name) {
            this.name = name;
            return this;
        }


        /** スタミナモデルマスターの説明 */
		[UnityEngine.SerializeField]
        public string description;

        /**
         * スタミナモデルマスターの説明を設定
         *
         * @param description スタミナモデルマスターの説明
         * @return this
         */
        public CreateStaminaModelMasterRequest WithDescription(string description) {
            this.description = description;
            return this;
        }


        /** スタミナの種類のメタデータ */
		[UnityEngine.SerializeField]
        public string metadata;

        /**
         * スタミナの種類のメタデータを設定
         *
         * @param metadata スタミナの種類のメタデータ
         * @return this
         */
        public CreateStaminaModelMasterRequest WithMetadata(string metadata) {
            this.metadata = metadata;
            return this;
        }


        /** スタミナを回復する速度(分) */
		[UnityEngine.SerializeField]
        public int? recoverIntervalMinutes;

        /**
         * スタミナを回復する速度(分)を設定
         *
         * @param recoverIntervalMinutes スタミナを回復する速度(分)
         * @return this
         */
        public CreateStaminaModelMasterRequest WithRecoverIntervalMinutes(int? recoverIntervalMinutes) {
            this.recoverIntervalMinutes = recoverIntervalMinutes;
            return this;
        }


        /** 時間経過後に回復する量 */
		[UnityEngine.SerializeField]
        public int? recoverValue;

        /**
         * 時間経過後に回復する量を設定
         *
         * @param recoverValue 時間経過後に回復する量
         * @return this
         */
        public CreateStaminaModelMasterRequest WithRecoverValue(int? recoverValue) {
            this.recoverValue = recoverValue;
            return this;
        }


        /** スタミナの最大値の初期値 */
		[UnityEngine.SerializeField]
        public int? initialCapacity;

        /**
         * スタミナの最大値の初期値を設定
         *
         * @param initialCapacity スタミナの最大値の初期値
         * @return this
         */
        public CreateStaminaModelMasterRequest WithInitialCapacity(int? initialCapacity) {
            this.initialCapacity = initialCapacity;
            return this;
        }


        /** 最大値を超えて回復するか */
		[UnityEngine.SerializeField]
        public bool? isOverflow;

        /**
         * 最大値を超えて回復するかを設定
         *
         * @param isOverflow 最大値を超えて回復するか
         * @return this
         */
        public CreateStaminaModelMasterRequest WithIsOverflow(bool? isOverflow) {
            this.isOverflow = isOverflow;
            return this;
        }


        /** 溢れた状況での最大値 */
		[UnityEngine.SerializeField]
        public int? maxCapacity;

        /**
         * 溢れた状況での最大値を設定
         *
         * @param maxCapacity 溢れた状況での最大値
         * @return this
         */
        public CreateStaminaModelMasterRequest WithMaxCapacity(int? maxCapacity) {
            this.maxCapacity = maxCapacity;
            return this;
        }


        /** GS2-Experience のランクによって最大スタミナ値を決定するスタミナ最大値テーブル名 */
		[UnityEngine.SerializeField]
        public string maxStaminaTableName;

        /**
         * GS2-Experience のランクによって最大スタミナ値を決定するスタミナ最大値テーブル名を設定
         *
         * @param maxStaminaTableName GS2-Experience のランクによって最大スタミナ値を決定するスタミナ最大値テーブル名
         * @return this
         */
        public CreateStaminaModelMasterRequest WithMaxStaminaTableName(string maxStaminaTableName) {
            this.maxStaminaTableName = maxStaminaTableName;
            return this;
        }


        /** GS2-Experience のランクによってスタミナの回復間隔を決定する回復間隔テーブル名 */
		[UnityEngine.SerializeField]
        public string recoverIntervalTableName;

        /**
         * GS2-Experience のランクによってスタミナの回復間隔を決定する回復間隔テーブル名を設定
         *
         * @param recoverIntervalTableName GS2-Experience のランクによってスタミナの回復間隔を決定する回復間隔テーブル名
         * @return this
         */
        public CreateStaminaModelMasterRequest WithRecoverIntervalTableName(string recoverIntervalTableName) {
            this.recoverIntervalTableName = recoverIntervalTableName;
            return this;
        }


        /** GS2-Experience のランクによってスタミナの回復量を決定する回復量テーブル名 */
		[UnityEngine.SerializeField]
        public string recoverValueTableName;

        /**
         * GS2-Experience のランクによってスタミナの回復量を決定する回復量テーブル名を設定
         *
         * @param recoverValueTableName GS2-Experience のランクによってスタミナの回復量を決定する回復量テーブル名
         * @return this
         */
        public CreateStaminaModelMasterRequest WithRecoverValueTableName(string recoverValueTableName) {
            this.recoverValueTableName = recoverValueTableName;
            return this;
        }


    	[Preserve]
        public static CreateStaminaModelMasterRequest FromDict(JsonData data)
        {
            return new CreateStaminaModelMasterRequest {
                namespaceName = data.Keys.Contains("namespaceName") && data["namespaceName"] != null ? data["namespaceName"].ToString(): null,
                name = data.Keys.Contains("name") && data["name"] != null ? data["name"].ToString(): null,
                description = data.Keys.Contains("description") && data["description"] != null ? data["description"].ToString(): null,
                metadata = data.Keys.Contains("metadata") && data["metadata"] != null ? data["metadata"].ToString(): null,
                recoverIntervalMinutes = data.Keys.Contains("recoverIntervalMinutes") && data["recoverIntervalMinutes"] != null ? (int?)int.Parse(data["recoverIntervalMinutes"].ToString()) : null,
                recoverValue = data.Keys.Contains("recoverValue") && data["recoverValue"] != null ? (int?)int.Parse(data["recoverValue"].ToString()) : null,
                initialCapacity = data.Keys.Contains("initialCapacity") && data["initialCapacity"] != null ? (int?)int.Parse(data["initialCapacity"].ToString()) : null,
                isOverflow = data.Keys.Contains("isOverflow") && data["isOverflow"] != null ? (bool?)bool.Parse(data["isOverflow"].ToString()) : null,
                maxCapacity = data.Keys.Contains("maxCapacity") && data["maxCapacity"] != null ? (int?)int.Parse(data["maxCapacity"].ToString()) : null,
                maxStaminaTableName = data.Keys.Contains("maxStaminaTableName") && data["maxStaminaTableName"] != null ? data["maxStaminaTableName"].ToString(): null,
                recoverIntervalTableName = data.Keys.Contains("recoverIntervalTableName") && data["recoverIntervalTableName"] != null ? data["recoverIntervalTableName"].ToString(): null,
                recoverValueTableName = data.Keys.Contains("recoverValueTableName") && data["recoverValueTableName"] != null ? data["recoverValueTableName"].ToString(): null,
            };
        }

	}
}