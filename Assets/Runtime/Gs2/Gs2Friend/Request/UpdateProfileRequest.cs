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
using Gs2.Gs2Friend.Model;
using LitJson;
using UnityEngine.Scripting;

namespace Gs2.Gs2Friend.Request
{
	[Preserve]
	[System.Serializable]
	public class UpdateProfileRequest : Gs2Request<UpdateProfileRequest>
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
        public UpdateProfileRequest WithNamespaceName(string namespaceName) {
            this.namespaceName = namespaceName;
            return this;
        }


        /** 公開されるプロフィール */
		[UnityEngine.SerializeField]
        public string publicProfile;

        /**
         * 公開されるプロフィールを設定
         *
         * @param publicProfile 公開されるプロフィール
         * @return this
         */
        public UpdateProfileRequest WithPublicProfile(string publicProfile) {
            this.publicProfile = publicProfile;
            return this;
        }


        /** フォロワー向けに公開されるプロフィール */
		[UnityEngine.SerializeField]
        public string followerProfile;

        /**
         * フォロワー向けに公開されるプロフィールを設定
         *
         * @param followerProfile フォロワー向けに公開されるプロフィール
         * @return this
         */
        public UpdateProfileRequest WithFollowerProfile(string followerProfile) {
            this.followerProfile = followerProfile;
            return this;
        }


        /** フレンド向けに公開されるプロフィール */
		[UnityEngine.SerializeField]
        public string friendProfile;

        /**
         * フレンド向けに公開されるプロフィールを設定
         *
         * @param friendProfile フレンド向けに公開されるプロフィール
         * @return this
         */
        public UpdateProfileRequest WithFriendProfile(string friendProfile) {
            this.friendProfile = friendProfile;
            return this;
        }


        /** 重複実行回避機能に使用するID */
		[UnityEngine.SerializeField]
        public string duplicationAvoider;

        /**
         * 重複実行回避機能に使用するIDを設定
         *
         * @param duplicationAvoider 重複実行回避機能に使用するID
         * @return this
         */
        public UpdateProfileRequest WithDuplicationAvoider(string duplicationAvoider) {
            this.duplicationAvoider = duplicationAvoider;
            return this;
        }


        /** アクセストークン */
        public string accessToken { set; get; }

        /**
         * アクセストークンを設定
         *
         * @param accessToken アクセストークン
         * @return this
         */
        public UpdateProfileRequest WithAccessToken(string accessToken) {
            this.accessToken = accessToken;
            return this;
        }

    	[Preserve]
        public static UpdateProfileRequest FromDict(JsonData data)
        {
            return new UpdateProfileRequest {
                namespaceName = data.Keys.Contains("namespaceName") && data["namespaceName"] != null ? data["namespaceName"].ToString(): null,
                publicProfile = data.Keys.Contains("publicProfile") && data["publicProfile"] != null ? data["publicProfile"].ToString(): null,
                followerProfile = data.Keys.Contains("followerProfile") && data["followerProfile"] != null ? data["followerProfile"].ToString(): null,
                friendProfile = data.Keys.Contains("friendProfile") && data["friendProfile"] != null ? data["friendProfile"].ToString(): null,
                duplicationAvoider = data.Keys.Contains("duplicationAvoider") && data["duplicationAvoider"] != null ? data["duplicationAvoider"].ToString(): null,
            };
        }

	}
}